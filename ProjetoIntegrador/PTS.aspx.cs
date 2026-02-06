using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProjetoIntegrador
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string idParam = Request.QueryString["id"];
                    if (string.IsNullOrEmpty(idParam))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert",
                            "alert('ERRO: ID do paciente não foi passado na URL!'); window.history.back();", true);
                        return;
                    }
                    int idPaciente = Convert.ToInt32(idParam);
                    hfIdPaciente.Value = idPaciente.ToString();
                    CarregarPaciente(idPaciente);
                    CarregarPTSAtual(idPaciente);
                    CarregarHistoricoPTS(idPaciente);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    $"alert('ERRO no Page_Load: {ex.Message}');", true);
            }
        }

        private int ObterIdPaciente()
        {
            if (!string.IsNullOrEmpty(hfIdPaciente.Value))
                return Convert.ToInt32(hfIdPaciente.Value);

            if (Request.QueryString["id"] != null)
                return Convert.ToInt32(Request.QueryString["id"]);

            throw new Exception("ID do paciente não encontrado");
        }

        // Carregar somente o PTS da internação aberta (se houver)
        private void CarregarPTSAtual(int idPaciente)
        {
            int idInternacao = -1;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sqlInt = "SELECT TOP 1 id_internacao FROM Internacoes WHERE id_paciente = @id AND data_alta IS NULL ORDER BY data_internacao DESC";
                SqlCommand cmdInt = new SqlCommand(sqlInt, con);
                cmdInt.Parameters.AddWithValue("@id", idPaciente);
                var result = cmdInt.ExecuteScalar();
                if (result != null)
                    idInternacao = Convert.ToInt32(result);
            }

            if (idInternacao != -1)
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    string sql = @"SELECT TOP 1 * FROM pts WHERE id_internacao = @idInt";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@idInt", idInternacao);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfIdPTS.Value = dr["id_pts"].ToString();
                        PreencherCamposPTS(dr);
                        SetEditavel(true);
                    }
                    else
                    {
                        LimparCamposPTS();
                        SetEditavel(true);
                    }
                    dr.Close();
                }
            }
            else
            {
                LimparCamposPTS();
                SetEditavel(false);
            }
        }

        protected void gvPts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idPTS = (int)gvPts.SelectedDataKey.Value;
            int idPaciente = int.Parse(hfIdPaciente.Value);
            CarregarPaciente(idPaciente);

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sql = @"SELECT p.*, i.data_alta 
                               FROM pts p 
                               INNER JOIN Internacoes i ON p.id_internacao = i.id_internacao 
                               WHERE p.id_pts = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPTS);

                var dr = cmd.ExecuteReader();
                bool editavel = false;
                if (dr.Read())
                {
                    hfIdPTS.Value = dr["id_pts"].ToString();
                    PreencherCamposPTS(dr);
                    editavel = dr["data_alta"] == DBNull.Value;
                }
                dr.Close();
                SetEditavel(editavel);
            }
        }

        // SALVAR E ATUALIZAR
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int idPaciente = ObterIdPaciente();

                string curtoPrazo = ConcatenarAcoesCurtoPrazo();
                string medioPrazo = ConcatenarAcoesMedioPrazo();
                string longoPrazo = ConcatenarAcoesLongoPrazo();

                // Busca a internação ativa
                int idInternacao = -1;
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT TOP 1 id_internacao FROM Internacoes WHERE id_paciente = @id AND data_alta IS NULL ORDER BY data_internacao DESC", con);
                    cmd.Parameters.AddWithValue("@id", idPaciente);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null)
                        idInternacao = (int)obj;
                }

                if (idInternacao == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Não existe internação ativa para esse paciente!');", true);
                    return;
                }

                if (!string.IsNullOrEmpty(hfIdPTS.Value))
                {
                    // Atualiza PTS existente
                    int idPTS = Convert.ToInt32(hfIdPTS.Value);
                    AtualizarPTS(idPTS, idPaciente, idInternacao, curtoPrazo, medioPrazo, longoPrazo);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('PTS atualizado com sucesso!'); window.location='Home.aspx';", true);
                }
                else
                {
                    // Salva novo PTS
                    SalvarPTS(idPaciente, idInternacao, curtoPrazo, medioPrazo, longoPrazo);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('PTS salvo com sucesso!'); window.location='Home.aspx';", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Erro: " + ex.Message + "');", true);
            }
        }

        private void SalvarPTS(int idPaciente, int idInternacao, string curtoPrazo, string medioPrazo, string longoPrazo)
        {
            string query = @"INSERT INTO pts 
            (id_paciente, id_internacao, enfermeiro, medica, nutricionista, fisioterapeuta, psicologa, 
             medicamentos, historia, enfermagem, fisioterapia, nutricao, 
             pa, inspecao, avaliacao_fisica, grau_mobilidade, forca_sensibilidade, nivel_dependencia,
             curto_prazo, medio_prazo, longo_prazo, data_criacao)
            VALUES 
            (@id_paciente, @id_internacao, @enfermeiro, @medica, @nutricionista, @fisioterapeuta, @psicologa,
             @medicamentos, @historia, @enfermagem, @fisioterapia, @nutricao,
             @pa, @inspecao, @avaliacao_fisica, @grau_mobilidade, @forca_sensibilidade, @nivel_dependencia,
             @curto_prazo, @medio_prazo, @longo_prazo, GETDATE())";

            using (SqlConnection conn = new SqlConnection(strCon))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_paciente", idPaciente);
                    cmd.Parameters.AddWithValue("@id_internacao", idInternacao);
                    cmd.Parameters.AddWithValue("@enfermeiro", inputEnfermeiro.Text.Trim());
                    cmd.Parameters.AddWithValue("@medica", inputMedica.Text.Trim());
                    cmd.Parameters.AddWithValue("@nutricionista", inputNutri.Text.Trim());
                    cmd.Parameters.AddWithValue("@fisioterapeuta", inputFisio.Text.Trim());
                    cmd.Parameters.AddWithValue("@psicologa", inputPsico.Text.Trim());
                    cmd.Parameters.AddWithValue("@medicamentos", inputContinuo.Text.Trim());
                    cmd.Parameters.AddWithValue("@historia", inputPregressa.Text.Trim());
                    cmd.Parameters.AddWithValue("@enfermagem", inputEnfermagem.Text.Trim());
                    cmd.Parameters.AddWithValue("@fisioterapia", inputFisioterapia.Text.Trim());
                    cmd.Parameters.AddWithValue("@nutricao", inputNutricao.Text.Trim());
                    cmd.Parameters.AddWithValue("@pa", inputPA.Text.Trim());
                    cmd.Parameters.AddWithValue("@inspecao", inputInspecao.Text.Trim());
                    cmd.Parameters.AddWithValue("@avaliacao_fisica", inputAvaliacao.Text.Trim());
                    cmd.Parameters.AddWithValue("@grau_mobilidade", inputGrau.Text.Trim());
                    cmd.Parameters.AddWithValue("@forca_sensibilidade", inputForca.Text.Trim());
                    cmd.Parameters.AddWithValue("@nivel_dependencia", inputDependencia.Text.Trim());
                    cmd.Parameters.AddWithValue("@curto_prazo", curtoPrazo);
                    cmd.Parameters.AddWithValue("@medio_prazo", medioPrazo);
                    cmd.Parameters.AddWithValue("@longo_prazo", longoPrazo);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void AtualizarPTS(int idPTS, int idPaciente, int idInternacao, string curtoPrazo, string medioPrazo, string longoPrazo)
        {
            string query = @"UPDATE pts SET
            id_paciente = @id_paciente,
            id_internacao = @id_internacao,
            enfermeiro = @enfermeiro, medica = @medica, nutricionista = @nutricionista,
            fisioterapeuta = @fisioterapeuta, psicologa = @psicologa,
            medicamentos = @medicamentos, historia = @historia,
            enfermagem = @enfermagem, fisioterapia = @fisioterapia, nutricao = @nutricao,
            pa = @pa, inspecao = @inspecao, avaliacao_fisica = @avaliacao_fisica,
            grau_mobilidade = @grau_mobilidade, forca_sensibilidade = @forca_sensibilidade,
            nivel_dependencia = @nivel_dependencia,
            curto_prazo = @curto_prazo, medio_prazo = @medio_prazo, longo_prazo = @longo_prazo
            WHERE id_pts = @id_pts";

            using (SqlConnection conn = new SqlConnection(strCon))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id_pts", idPTS);
                    cmd.Parameters.AddWithValue("@id_paciente", idPaciente);
                    cmd.Parameters.AddWithValue("@id_internacao", idInternacao);
                    cmd.Parameters.AddWithValue("@enfermeiro", inputEnfermeiro.Text.Trim());
                    cmd.Parameters.AddWithValue("@medica", inputMedica.Text.Trim());
                    cmd.Parameters.AddWithValue("@nutricionista", inputNutri.Text.Trim());
                    cmd.Parameters.AddWithValue("@fisioterapeuta", inputFisio.Text.Trim());
                    cmd.Parameters.AddWithValue("@psicologa", inputPsico.Text.Trim());
                    cmd.Parameters.AddWithValue("@medicamentos", inputContinuo.Text.Trim());
                    cmd.Parameters.AddWithValue("@historia", inputPregressa.Text.Trim());
                    cmd.Parameters.AddWithValue("@enfermagem", inputEnfermagem.Text.Trim());
                    cmd.Parameters.AddWithValue("@fisioterapia", inputFisioterapia.Text.Trim());
                    cmd.Parameters.AddWithValue("@nutricao", inputNutricao.Text.Trim());
                    cmd.Parameters.AddWithValue("@pa", inputPA.Text.Trim());
                    cmd.Parameters.AddWithValue("@inspecao", inputInspecao.Text.Trim());
                    cmd.Parameters.AddWithValue("@avaliacao_fisica", inputAvaliacao.Text.Trim());
                    cmd.Parameters.AddWithValue("@grau_mobilidade", inputGrau.Text.Trim());
                    cmd.Parameters.AddWithValue("@forca_sensibilidade", inputForca.Text.Trim());
                    cmd.Parameters.AddWithValue("@nivel_dependencia", inputDependencia.Text.Trim());
                    cmd.Parameters.AddWithValue("@curto_prazo", curtoPrazo);
                    cmd.Parameters.AddWithValue("@medio_prazo", medioPrazo);
                    cmd.Parameters.AddWithValue("@longo_prazo", longoPrazo);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string ConcatenarAcoesCurtoPrazo()
        {
            string acoes = "";
            if (!string.IsNullOrWhiteSpace(txtCurtoEnfermagem.Text))
                acoes += "ENFERMAGEM:\n" + txtCurtoEnfermagem.Text.Trim() + "\n\n";
            if (!string.IsNullOrWhiteSpace(txtCurtoFisio.Text))
                acoes += "FISIOTERAPIA:\n" + txtCurtoFisio.Text.Trim() + "\n\n";
            if (!string.IsNullOrWhiteSpace(txtCurtoNutri.Text))
                acoes += "NUTRIÇÃO:\n" + txtCurtoNutri.Text.Trim();
            return acoes.Trim();
        }
        private string ConcatenarAcoesMedioPrazo()
        {
            string acoes = "";
            if (!string.IsNullOrWhiteSpace(txtMedioEnfermagem.Text))
                acoes += "ENFERMAGEM:\n" + txtMedioEnfermagem.Text.Trim() + "\n\n";
            if (!string.IsNullOrWhiteSpace(txtMedioFisio.Text))
                acoes += "FISIOTERAPIA:\n" + txtMedioFisio.Text.Trim() + "\n\n";
            if (!string.IsNullOrWhiteSpace(txtMedioNutri.Text))
                acoes += "NUTRIÇÃO:\n" + txtMedioNutri.Text.Trim();
            return acoes.Trim();
        }
        private string ConcatenarAcoesLongoPrazo()
        {
            string acoes = "";
            if (!string.IsNullOrWhiteSpace(txtLongoEnfermagem.Text))
                acoes += "ENFERMAGEM:\n" + txtLongoEnfermagem.Text.Trim() + "\n\n";
            if (!string.IsNullOrWhiteSpace(txtLongoFisio.Text))
                acoes += "FISIOTERAPIA:\n" + txtLongoFisio.Text.Trim() + "\n\n";
            if (!string.IsNullOrWhiteSpace(txtLongoNutri.Text))
                acoes += "NUTRIÇÃO:\n" + txtLongoNutri.Text.Trim();
            return acoes.Trim();
        }

        private void CarregarPaciente(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    string sql = "SELECT nome, nascimento, endereco, esf FROM paciente WHERE id_paciente=@id";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string nome = dr["nome"].ToString();
                        string endereco = dr["endereco"].ToString();
                        string esf = dr["esf"].ToString();

                        DateTime nascimento = Convert.ToDateTime(dr["nascimento"]);
                        int idade = DateTime.Now.Year - nascimento.Year;
                        if (DateTime.Now.DayOfYear < nascimento.DayOfYear) idade--;

                        inputPaciente.Text = nome;
                        inputEndereco.Text = endereco;
                        inputESF.Text = esf;
                        inputIdade.Text = idade.ToString();
                        hfNomePaciente.Value = nome;
                        hfIdadePaciente.Value = idade.ToString();
                        hfEnderecoPaciente.Value = endereco;
                        hfESFPaciente.Value = esf;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    $"alert('ERRO ao carregar paciente: {ex.Message}');", true);
            }
        }

        private void PreencherCamposPTS(SqlDataReader dr)
        {
            inputEnfermeiro.Text = dr["enfermeiro"]?.ToString() ?? "";
            inputMedica.Text = dr["medica"]?.ToString() ?? "";
            inputNutri.Text = dr["nutricionista"]?.ToString() ?? "";
            inputFisio.Text = dr["fisioterapeuta"]?.ToString() ?? "";
            inputPsico.Text = dr["psicologa"]?.ToString() ?? "";
            inputContinuo.Text = dr["medicamentos"]?.ToString() ?? "";
            inputPregressa.Text = dr["historia"]?.ToString() ?? "";
            inputEnfermagem.Text = dr["enfermagem"]?.ToString() ?? "";
            inputFisioterapia.Text = dr["fisioterapia"]?.ToString() ?? "";
            inputNutricao.Text = dr["nutricao"]?.ToString() ?? "";
            inputPA.Text = dr["pa"]?.ToString() ?? "";
            inputInspecao.Text = dr["inspecao"]?.ToString() ?? "";
            inputAvaliacao.Text = dr["avaliacao_fisica"]?.ToString() ?? "";
            inputGrau.Text = dr["grau_mobilidade"]?.ToString() ?? "";
            inputForca.Text = dr["forca_sensibilidade"]?.ToString() ?? "";
            inputDependencia.Text = dr["nivel_dependencia"]?.ToString() ?? "";

            PreencherAcoesPorPrazo(dr["curto_prazo"]?.ToString(), "curto");
            PreencherAcoesPorPrazo(dr["medio_prazo"]?.ToString(), "medio");
            PreencherAcoesPorPrazo(dr["longo_prazo"]?.ToString(), "longo");
        }

        private void LimparCamposPTS()
        {
            inputEnfermeiro.Text = "";
            inputMedica.Text = "";
            inputNutri.Text = "";
            inputFisio.Text = "";
            inputPsico.Text = "";
            inputContinuo.Text = "";
            inputPregressa.Text = "";
            inputEnfermagem.Text = "";
            inputFisioterapia.Text = "";
            inputNutricao.Text = "";
            inputPA.Text = "";
            inputInspecao.Text = "";
            inputAvaliacao.Text = "";
            inputGrau.Text = "";
            inputForca.Text = "";
            inputDependencia.Text = "";
            txtCurtoEnfermagem.Text = "";
            txtCurtoFisio.Text = "";
            txtCurtoNutri.Text = "";
            txtMedioEnfermagem.Text = "";
            txtMedioFisio.Text = "";
            txtMedioNutri.Text = "";
            txtLongoEnfermagem.Text = "";
            txtLongoFisio.Text = "";
            txtLongoNutri.Text = "";
        }

        private void SetEditavel(bool editavel)
        {
            inputEnfermeiro.ReadOnly = !editavel;
            inputMedica.ReadOnly = !editavel;
            inputNutri.ReadOnly = !editavel;
            inputFisio.ReadOnly = !editavel;
            inputPsico.ReadOnly = !editavel;
            inputContinuo.ReadOnly = !editavel;
            inputPregressa.ReadOnly = !editavel;
            inputEnfermagem.ReadOnly = !editavel;
            inputFisioterapia.ReadOnly = !editavel;
            inputNutricao.ReadOnly = !editavel;
            inputPA.ReadOnly = !editavel;
            inputInspecao.ReadOnly = !editavel;
            inputAvaliacao.ReadOnly = !editavel;
            inputGrau.ReadOnly = !editavel;
            inputForca.ReadOnly = !editavel;
            inputDependencia.ReadOnly = !editavel;
            txtCurtoEnfermagem.ReadOnly = !editavel;
            txtCurtoFisio.ReadOnly = !editavel;
            txtCurtoNutri.ReadOnly = !editavel;
            txtMedioEnfermagem.ReadOnly = !editavel;
            txtMedioFisio.ReadOnly = !editavel;
            txtMedioNutri.ReadOnly = !editavel;
            txtLongoEnfermagem.ReadOnly = !editavel;
            txtLongoFisio.ReadOnly = !editavel;
            txtLongoNutri.ReadOnly = !editavel;
            Button1.Enabled = editavel;
        }

        private void CarregarHistoricoPTS(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                SELECT 
                    p.id_pts,
                    p.enfermeiro,
                    p.data_criacao,
                    i.setor,
                    i.data_alta,
                    CASE WHEN i.data_alta IS NULL THEN 'Editável' ELSE 'Somente leitura' END as status
                FROM pts p
                INNER JOIN Internacoes i ON p.id_internacao = i.id_internacao
                WHERE i.id_paciente = @id
                ORDER BY i.data_internacao DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                var dt = new System.Data.DataTable();
                dt.Load(cmd.ExecuteReader());
                gvPts.DataSource = dt;
                gvPts.DataBind();
            }
        }

        private void PreencherAcoesPorPrazo(string textoCompleto, string prazo)
        {
            if (string.IsNullOrWhiteSpace(textoCompleto)) return;

            string enfermagem = ExtrairSecao(textoCompleto, "ENFERMAGEM:");
            string fisioterapia = ExtrairSecao(textoCompleto, "FISIOTERAPIA:");
            string nutricao = ExtrairSecao(textoCompleto, "NUTRIÇÃO:");

            switch (prazo.ToLower())
            {
                case "curto":
                    txtCurtoEnfermagem.Text = enfermagem;
                    txtCurtoFisio.Text = fisioterapia;
                    txtCurtoNutri.Text = nutricao;
                    break;
                case "medio":
                    txtMedioEnfermagem.Text = enfermagem;
                    txtMedioFisio.Text = fisioterapia;
                    txtMedioNutri.Text = nutricao;
                    break;
                case "longo":
                    txtLongoEnfermagem.Text = enfermagem;
                    txtLongoFisio.Text = fisioterapia;
                    txtLongoNutri.Text = nutricao;
                    break;
            }
        }

        private string ExtrairSecao(string textoCompleto, string marcador)
        {
            if (string.IsNullOrWhiteSpace(textoCompleto) || !textoCompleto.Contains(marcador)) return "";

            int inicio = textoCompleto.IndexOf(marcador) + marcador.Length;
            string resto = textoCompleto.Substring(inicio);

            string[] marcadores = { "ENFERMAGEM:", "FISIOTERAPIA:", "NUTRIÇÃO:" };
            int proximoMarcador = resto.Length;

            foreach (string m in marcadores)
            {
                int pos = resto.IndexOf(m);
                if (pos > 0 && pos < proximoMarcador)
                {
                    proximoMarcador = pos;
                }
            }

            return resto.Substring(0, proximoMarcador).Trim();
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void btnGerarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(document, ms);

                document.Open();

                // Fontes
                Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
                Font fontSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK);
                Font fontNegrito = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.BLACK);
                Font fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
                Font fontPequena = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);

                // Título
                Paragraph titulo = new Paragraph("PLANO TERAPÊUTICO SINGULAR (PTS)", fontTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 20f;
                document.Add(titulo);

                // 👇 DADOS DO PACIENTE - USANDO HIDDENFIELDS
                document.Add(new Paragraph("Paciente: " + hfNomePaciente.Value, fontNegrito));
                document.Add(new Paragraph("Idade: " + hfIdadePaciente.Value + " anos", fontNormal));
                document.Add(new Paragraph("Endereço: " + hfEnderecoPaciente.Value, fontNormal));
                document.Add(new Paragraph("ESF: " + hfESFPaciente.Value, fontNormal));
                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph("─────────────────────────────────────────────", fontNormal));
                document.Add(Chunk.NEWLINE);

                // Equipe
                document.Add(new Paragraph("EQUIPE MULTIPROFISSIONAL", fontSubtitulo));
                document.Add(Chunk.NEWLINE);

                if (!string.IsNullOrWhiteSpace(inputEnfermeiro.Text))
                    document.Add(new Paragraph("Enfermeiro: " + inputEnfermeiro.Text, fontNormal));
                if (!string.IsNullOrWhiteSpace(inputMedica.Text))
                    document.Add(new Paragraph("Médica: " + inputMedica.Text, fontNormal));
                if (!string.IsNullOrWhiteSpace(inputNutri.Text))
                    document.Add(new Paragraph("Nutricionista: " + inputNutri.Text, fontNormal));
                if (!string.IsNullOrWhiteSpace(inputFisio.Text))
                    document.Add(new Paragraph("Fisioterapeuta: " + inputFisio.Text, fontNormal));
                if (!string.IsNullOrWhiteSpace(inputPsico.Text))
                    document.Add(new Paragraph("Psicóloga: " + inputPsico.Text, fontNormal));
                document.Add(Chunk.NEWLINE);

                // Informações Clínicas
                if (!string.IsNullOrWhiteSpace(inputContinuo.Text))
                {
                    document.Add(new Paragraph("Medicamentos de Uso Contínuo:", fontNegrito));
                    document.Add(new Paragraph(inputContinuo.Text, fontNormal));
                    document.Add(Chunk.NEWLINE);
                }
                if (!string.IsNullOrWhiteSpace(inputPregressa.Text))
                {
                    document.Add(new Paragraph("História Pregressa:", fontNegrito));
                    document.Add(new Paragraph(inputPregressa.Text, fontNormal));
                    document.Add(Chunk.NEWLINE);
                }

                if (!string.IsNullOrWhiteSpace(inputEnfermagem.Text))
                {
                    document.Add(new Paragraph("ENFERMAGEM", fontNegrito));
                    document.Add(new Paragraph(inputEnfermagem.Text, fontNormal));
                    document.Add(Chunk.NEWLINE);
                }
                if (!string.IsNullOrWhiteSpace(inputFisioterapia.Text))
                {
                    document.Add(new Paragraph("FISIOTERAPIA", fontNegrito));
                    document.Add(new Paragraph(inputFisioterapia.Text, fontNormal));
                    if (!string.IsNullOrWhiteSpace(inputPA.Text))
                        document.Add(new Paragraph("PA: " + inputPA.Text, fontNormal));
                    if (!string.IsNullOrWhiteSpace(inputInspecao.Text))
                        document.Add(new Paragraph("Inspeção: " + inputInspecao.Text, fontNormal));
                    if (!string.IsNullOrWhiteSpace(inputAvaliacao.Text))
                        document.Add(new Paragraph("Avaliação Física: " + inputAvaliacao.Text, fontNormal));
                    if (!string.IsNullOrWhiteSpace(inputGrau.Text))
                        document.Add(new Paragraph("Grau de Mobilidade: " + inputGrau.Text, fontNormal));
                    if (!string.IsNullOrWhiteSpace(inputForca.Text))
                        document.Add(new Paragraph("Força e Sensibilidade: " + inputForca.Text, fontNormal));
                    if (!string.IsNullOrWhiteSpace(inputDependencia.Text))
                        document.Add(new Paragraph("Nível de Dependência: " + inputDependencia.Text, fontNormal));
                    document.Add(Chunk.NEWLINE);
                }
                if (!string.IsNullOrWhiteSpace(inputNutricao.Text))
                {
                    document.Add(new Paragraph("NUTRIÇÃO", fontNegrito));
                    document.Add(new Paragraph(inputNutricao.Text, fontNormal));
                    document.Add(Chunk.NEWLINE);
                }

                document.Add(new Paragraph("─────────────────────────────────────────────", fontNormal));
                document.Add(Chunk.NEWLINE);

                // PRAZOS
                document.Add(new Paragraph("AÇÕES DE CURTO PRAZO", fontSubtitulo));
                document.Add(Chunk.NEWLINE);
                AdicionarAcoesPrazo(document, txtCurtoEnfermagem.Text, txtCurtoFisio.Text, txtCurtoNutri.Text, fontNegrito, fontPequena);

                document.Add(new Paragraph("AÇÕES DE MÉDIO PRAZO", fontSubtitulo));
                document.Add(Chunk.NEWLINE);
                AdicionarAcoesPrazo(document, txtMedioEnfermagem.Text, txtMedioFisio.Text, txtMedioNutri.Text, fontNegrito, fontPequena);

                document.Add(new Paragraph("AÇÕES DE LONGO PRAZO", fontSubtitulo));
                document.Add(Chunk.NEWLINE);
                AdicionarAcoesPrazo(document, txtLongoEnfermagem.Text, txtLongoFisio.Text, txtLongoNutri.Text, fontNegrito, fontPequena);

                // Data
                Paragraph data = new Paragraph("\nData: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fontNormal);
                data.Alignment = Element.ALIGN_RIGHT;
                document.Add(data);

                document.Close();

                byte[] pdfBytes = ms.ToArray();
                ms.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=PTS_" + hfNomePaciente.Value.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Erro ao gerar PDF: " + ex.Message + "');", true);
            }
        }
            private void AdicionarAcoesPrazo(Document document, string enfermagem, string fisio, string nutricao, Font fontNegrito, Font fontNormal)
        {
            if (!string.IsNullOrWhiteSpace(enfermagem))
            {
                document.Add(new Paragraph("Enfermagem:", fontNegrito));
                document.Add(new Paragraph(enfermagem, fontNormal));
                document.Add(Chunk.NEWLINE);
            }

            if (!string.IsNullOrWhiteSpace(fisio))
            {
                document.Add(new Paragraph("Fisioterapia:", fontNegrito));
                document.Add(new Paragraph(fisio, fontNormal));
                document.Add(Chunk.NEWLINE);
            }

            if (!string.IsNullOrWhiteSpace(nutricao))
            {
                document.Add(new Paragraph("Nutrição:", fontNegrito));
                document.Add(new Paragraph(nutricao, fontNormal));
                document.Add(Chunk.NEWLINE);
            }
        }

    }

}


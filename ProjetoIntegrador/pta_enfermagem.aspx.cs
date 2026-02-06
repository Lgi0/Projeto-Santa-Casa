
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjetoIntegrador
{
    public partial class pta_enfermagem : System.Web.UI.Page
    {
        
        string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            int idPaciente = -1;

            if (Request.QueryString["id"] != null)
            {
                idPaciente = Convert.ToInt32(Request.QueryString["id"]);
                Session["ID_PACIENTE"] = idPaciente; // Mantém na session se quiser
            }
            else if (Session["ID_PACIENTE"] != null)
            {
                idPaciente = (int)Session["ID_PACIENTE"];
            }
            else
            {
                Response.Redirect("Home.aspx"); // Sem paciente, volta!
                return;
            }

            if (!IsPostBack)
            {
                CarregarPaciente(idPaciente);
                CarregarListaPTAs(idPaciente);
            }
        }

       


        private void CarregarListaPTAs(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                              SELECT 
                                pta.id_pta_enfermagem, 
                                i.data_internacao,
                                i.setor,
                                i.leito,
                                i.data_alta,
                                CASE WHEN i.data_alta IS NOT NULL THEN 'LIBERADO' ELSE 'EDITÁVEL' END as status
                              FROM pta_enfermagem pta 
                              INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
                              WHERE i.id_paciente = @id
                              ORDER BY i.data_internacao DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                var dt = new System.Data.DataTable();
                dt.Load(cmd.ExecuteReader());
                gvPtas.DataSource = dt;
                gvPtas.DataBind();
            }
        }

        protected void btnGerarPDF_Click(object sender, EventArgs e)
        {
            // Dados que vão para o PDF:
            string nome = txtNome.Text;
            string idade = txtIdade.Text;
            string setor = txtSetor.Text;
            string leito = txtLeito.Text;
            string medico = txtMedico.Text;
            string crm = txtCRM.Text;
            string dtInternacao = txtInternacao.Text;
            string dtAlta = txtAlta.Text;
            string hd = txtHD.Text;
            string glasgow = txtGlasgow.Text;
            string descricao = txtDescricao.Text;
            string iniciais = txtIniciais.Text;
            string internacaoDados = txtInternacaoDados.Text;
            string dadosAlta = txtDadosAlta.Text;

            // Checbox como "Sim"/"Não"
            string portadorDiabetes = chkDiabetes.Checked ? "Sim" : "Não";
            string portadorHAS = chkHAS.Checked ? "Sim" : "Não";
            string portadorClinico = chkClinico.Checked ? "Sim" : "Não";
            string portadorCirurgico = chkCirurgico.Checked ? "Sim" : "Não";
            string portadorOutros = txtOutros.Text;

            string necessitaUlcera = chkUlcera.Checked ? "Sim" : "Não";
            string necessitaEstomas = chkEstomas.Checked ? "Sim" : "Não";
            string necessitaSonda = chkSonda.Checked ? "Sim" : "Não";
            string necessitaTraqueo = chkTraqueo.Checked ? "Sim" : "Não";
            string necessitaOxigenio = chkOxigenio.Checked ? "Sim" : "Não";
            string necessitaAspiracao = chkAspiracao.Checked ? "Sim" : "Não";
            string necessitaCurativos = chkCurativos.Checked ? "Sim" : "Não";
            string necessitaOutros = txtOutrosAlta.Text;

            // Geração do PDF:
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titlefont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLUE);
                var sectionfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var textfont = FontFactory.GetFont(FontFactory.HELVETICA, 11);

                doc.Add(new Paragraph("Plano Terapêutico para Alta - Enfermagem", titlefont));
                doc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 80;
                table.DefaultCell.Padding = 10;
                table.DefaultCell.BorderColor = BaseColor.WHITE;
                table.AddCell(new Phrase("Nome do paciente:", sectionfont));
                table.AddCell(new Phrase(nome, textfont));
                table.AddCell(new Phrase("Idade:", sectionfont));
                table.AddCell(new Phrase(idade, textfont));
                table.AddCell(new Phrase("Setor:", sectionfont));
                table.AddCell(new Phrase(setor, textfont));
                table.AddCell(new Phrase("Leito:", sectionfont));
                table.AddCell(new Phrase(leito, textfont));
                table.AddCell(new Phrase("Médico:", sectionfont));
                table.AddCell(new Phrase(medico, textfont));
                table.AddCell(new Phrase("CRM:", sectionfont));
                table.AddCell(new Phrase(crm, textfont));
                table.AddCell(new Phrase("Data da Internação:", sectionfont));
                table.AddCell(new Phrase(dtInternacao, textfont));
                table.AddCell(new Phrase("Data da Alta:", sectionfont));
                table.AddCell(new Phrase(dtAlta, textfont));
                table.AddCell(new Phrase("HD:", sectionfont));
                table.AddCell(new Phrase(hd, textfont));
                table.AddCell(new Phrase("Glasgow:", sectionfont));
                table.AddCell(new Phrase(glasgow, textfont));
                table.SpacingAfter = 10;
                doc.Add(table);

                doc.Add(new Paragraph("Paciente portador de:", sectionfont));
                doc.Add(new Paragraph($"Diabetes: {portadorDiabetes}", textfont));
                doc.Add(new Paragraph($"HAS: {portadorHAS}", textfont));
                doc.Add(new Paragraph($"Clínico: {portadorClinico}", textfont));
                doc.Add(new Paragraph($"Cir: {portadorCirurgico}", textfont));
                doc.Add(new Paragraph($"Outros: {portadorOutros}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Necessidades na alta:", sectionfont));
                doc.Add(new Paragraph($"Úlcera: {necessitaUlcera}", textfont));
                doc.Add(new Paragraph($"Estomas: {necessitaEstomas}", textfont));
                doc.Add(new Paragraph($"Sonda Vesical: {necessitaSonda}", textfont));
                doc.Add(new Paragraph($"Traqueostomia: {necessitaTraqueo}", textfont));
                doc.Add(new Paragraph($"Oxigênio: {necessitaOxigenio}", textfont));
                doc.Add(new Paragraph($"Aspiração: {necessitaAspiracao}", textfont));
                doc.Add(new Paragraph($"Curativos: {necessitaCurativos}", textfont));
                doc.Add(new Paragraph($"Outros: {necessitaOutros}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Descrição do caso clínico:", sectionfont));
                doc.Add(new Paragraph(descricao, textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Dados Iniciais:", sectionfont));
                doc.Add(new Paragraph(iniciais, textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Dados da Internação:", sectionfont));
                doc.Add(new Paragraph(internacaoDados, textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Dados da Alta:", sectionfont));
                doc.Add(new Paragraph(dadosAlta, textfont));
                doc.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=PTA_Enfermagem_{nome.Replace(" ", "_")}.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }
        private void CarregarPaciente(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                // Buscar dados do paciente
                string sqlPaciente = "SELECT nome, nascimento FROM paciente WHERE id_paciente=@id";
                SqlCommand cmdPac = new SqlCommand(sqlPaciente, con);
                cmdPac.Parameters.AddWithValue("@id", idPaciente);

                con.Open();
                SqlDataReader drPac = cmdPac.ExecuteReader();
                if (drPac.Read())
                {
                    txtNome.Text = drPac["nome"].ToString();

                    DateTime nascimento = Convert.ToDateTime(drPac["nascimento"]);
                    int idade = DateTime.Today.Year - nascimento.Year;
                    if (DateTime.Today < nascimento.AddYears(idade)) idade--;
                    txtIdade.Text = idade.ToString();
                }
                drPac.Close();

                // Primeiro: busca internação "ativa" (sem data_alta)
                string sqlInternacao = @"
            SELECT TOP 1 setor, leito, data_internacao, data_alta, id_internacao
            FROM Internacoes
            WHERE id_paciente = @id AND data_alta IS NULL
            ORDER BY data_internacao DESC";

                SqlCommand cmdInt = new SqlCommand(sqlInternacao, con);
                cmdInt.Parameters.AddWithValue("@id", idPaciente);
                SqlDataReader drInt = cmdInt.ExecuteReader();

                if (!drInt.Read())
                {
                    drInt.Close();
                    // Se não achar internação ativa, puxa a última (todas com alta)
                    sqlInternacao = @"
                SELECT TOP 1 setor, leito, data_internacao, data_alta, id_internacao
                FROM Internacoes
                WHERE id_paciente = @id
                ORDER BY data_internacao DESC";
                    cmdInt = new SqlCommand(sqlInternacao, con);
                    cmdInt.Parameters.AddWithValue("@id", idPaciente);
                    drInt = cmdInt.ExecuteReader();
                    if (!drInt.Read()) { drInt.Close(); con.Close(); return; } // paciente sem nenhuma internação
                }

                txtSetor.Text = drInt["setor"].ToString();
                txtLeito.Text = drInt["leito"].ToString();
                txtInternacao.Text = Convert.ToDateTime(drInt["data_internacao"]).ToString("yyyy-MM-dd");
                txtAlta.Text = drInt["data_alta"] != DBNull.Value
                    ? Convert.ToDateTime(drInt["data_alta"]).ToString("yyyy-MM-dd")
                    : "";

                ViewState["ID_INTERNACAO"] = drInt["id_internacao"];
                drInt.Close();
                con.Close();
            }
        }


        protected void btnEnfermagem_Click(object sender, EventArgs e)
        {
            var perfil = Session["UserGroup"] as string;
            if (perfil != null && perfil.ToLower().Contains("enfer"))
                Response.Redirect("pta_enfermagem.aspx");
            else
                Response.Redirect("AcessoNegado.aspx");
        }

        protected void btnFisioterapia_Click(object sender, EventArgs e)
        {
            var perfil = Session["UserGroup"] as string;
            if (perfil != null && perfil.ToLower().Contains("fisio"))
                Response.Redirect("pta_fisioterapia.aspx");
            else
                Response.Redirect("AcessoNegado.aspx");
        }

        protected void btnNutricao_Click(object sender, EventArgs e)
        {
            var perfil = Session["UserGroup"] as string;
            if (perfil != null && perfil.ToLower().Contains("nutri"))
                Response.Redirect("pta_nutricao.aspx");
            else
                Response.Redirect("AcessoNegado.aspx");
        }

        protected void btnPsicologia_Click(object sender, EventArgs e)
        {
            var perfil = Session["UserGroup"] as string;
            if (perfil != null && perfil.ToLower().Contains("psicó"))
                Response.Redirect("pta_psicologia.aspx");
            else
                Response.Redirect("AcessoNegado.aspx");
        }

        protected void btnSocial_Click(object sender, EventArgs e)
        {
            var perfil = Session["UserGroup"] as string;
            if (perfil != null && perfil.ToLower().Contains("social"))
                Response.Redirect("pta_social.aspx");
            else
                Response.Redirect("AcessoNegado.aspx");
        }
    


        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session.Remove("ID_PACIENTE");
            Response.Redirect("Home.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            int idInternacao = ViewState["ID_INTERNACAO"] != null ? Convert.ToInt32(ViewState["ID_INTERNACAO"]) : -1;
            if (idInternacao == -1)
            {
                // Garantir que o id está disponível!
                // Exiba uma mensagem de erro ou trate conforme necessário
                return;
            }

            // Captura os dados do formulário
            string medico = txtMedico.Text.Trim();
            string crm = txtCRM.Text.Trim();
            string hd = txtHD.Text.Trim();
            string glasgow = txtGlasgow.Text.Trim();
            string descricao = txtDescricao.Text.Trim();
            string iniciais = txtIniciais.Text.Trim();
            string internacaoDados = txtInternacaoDados.Text.Trim();
            string dadosAlta = txtDadosAlta.Text.Trim();
            string portadorOutros = txtOutros.Text.Trim();
            string necessitaOutros = txtOutrosAlta.Text.Trim();

            // Bools
            bool portadorDiabetes = chkDiabetes.Checked;
            bool portadorHAS = chkHAS.Checked;
            bool portadorClinico = chkClinico.Checked;
            bool portadorCirurgico = chkCirurgico.Checked;
            bool necessitaUlcera = chkUlcera.Checked;
            bool necessitaEstomas = chkEstomas.Checked;
            bool necessitaSonda = chkSonda.Checked;
            bool necessitaTraqueo = chkTraqueo.Checked;
            bool necessitaOxigenio = chkOxigenio.Checked;
            bool necessitaAspiracao = chkAspiracao.Checked;
            bool necessitaCurativos = chkCurativos.Checked;

            bool orientacaoPaciente = chkPaciente.Checked;
            bool orientacaoFamiliar = chkFamiliar.Checked;
            bool orientacaoOutro = chkOutro.Checked;

            bool tipoOriCurativos = chkCurativosOri.Checked;
            bool tipoOriPrevUlcera = chkPrevencaoUlcera.Checked;
            bool tipoOriSondas = chkSondas.Checked;
            bool tipoOriAspiracao = chkAspiracaoOri.Checked;
            bool tipoOriDieta = chkDieta.Checked;
            bool tipoOriPele = chkPele.Checked;

            // Verifica se já existe PTA para essa internação
            int idPta = -1;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sqlCheck = "SELECT id_pta_enfermagem FROM pta_enfermagem WHERE id_internacao = @idInternacao";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, con);
                cmdCheck.Parameters.AddWithValue("@idInternacao", idInternacao);
                var result = cmdCheck.ExecuteScalar();
                if (result != null)
                {
                    idPta = Convert.ToInt32(result);
                }

                if (idPta == -1)
                {
                    // INSERE NOVO PTA
                    string sqlInsert = @"INSERT INTO pta_enfermagem
            (id_paciente, id_internacao, medico, crm, hd, glasgow, descricao, dados_iniciais, dados_internacao, dados_alta, portador_diabetes, portador_has, portador_clinico, portador_cirurgico, portador_outros, necessita_ulcera, necessita_estomas, necessita_sonda, necessita_traqueo, necessita_oxigenio, necessita_aspiracao, necessita_curativos, necessita_outros, orientacao_paciente, orientacao_familiar, orientacao_outro, tipo_ori_curativos, tipo_ori_prevencao_ulcera, tipo_ori_sondas, tipo_ori_aspiracao, tipo_ori_dieta, tipo_ori_pele)
            VALUES
            (@idPaciente, @idInternacao, @medico, @crm, @hd, @glasgow, @descricao, @iniciais, @internacaoDados, @dadosAlta, @portadorDiabetes, @portadorHAS, @portadorClinico, @portadorCirurgico, @portadorOutros, @necessitaUlcera, @necessitaEstomas, @necessitaSonda, @necessitaTraqueo, @necessitaOxigenio, @necessitaAspiracao, @necessitaCurativos, @necessitaOutros, @orientacaoPaciente, @orientacaoFamiliar, @orientacaoOutro, @tipoOriCurativos, @tipoOriPrevUlcera, @tipoOriSondas, @tipoOriAspiracao, @tipoOriDieta, @tipoOriPele)";

                    SqlCommand cmd = new SqlCommand(sqlInsert, con);

                    cmd.Parameters.AddWithValue("@idPaciente", Session["ID_PACIENTE"]);
                    cmd.Parameters.AddWithValue("@idInternacao", idInternacao);
                    cmd.Parameters.AddWithValue("@medico", medico);
                    cmd.Parameters.AddWithValue("@crm", crm);
                    cmd.Parameters.AddWithValue("@hd", hd);
                    cmd.Parameters.AddWithValue("@glasgow", glasgow);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@iniciais", iniciais);
                    cmd.Parameters.AddWithValue("@internacaoDados", internacaoDados);
                    cmd.Parameters.AddWithValue("@dadosAlta", dadosAlta);
                    cmd.Parameters.AddWithValue("@portadorDiabetes", portadorDiabetes);
                    cmd.Parameters.AddWithValue("@portadorHAS", portadorHAS);
                    cmd.Parameters.AddWithValue("@portadorClinico", portadorClinico);
                    cmd.Parameters.AddWithValue("@portadorCirurgico", portadorCirurgico);
                    cmd.Parameters.AddWithValue("@portadorOutros", portadorOutros);
                    cmd.Parameters.AddWithValue("@necessitaUlcera", necessitaUlcera);
                    cmd.Parameters.AddWithValue("@necessitaEstomas", necessitaEstomas);
                    cmd.Parameters.AddWithValue("@necessitaSonda", necessitaSonda);
                    cmd.Parameters.AddWithValue("@necessitaTraqueo", necessitaTraqueo);
                    cmd.Parameters.AddWithValue("@necessitaOxigenio", necessitaOxigenio);
                    cmd.Parameters.AddWithValue("@necessitaAspiracao", necessitaAspiracao);
                    cmd.Parameters.AddWithValue("@necessitaCurativos", necessitaCurativos);
                    cmd.Parameters.AddWithValue("@necessitaOutros", necessitaOutros);
                    cmd.Parameters.AddWithValue("@orientacaoPaciente", orientacaoPaciente);
                    cmd.Parameters.AddWithValue("@orientacaoFamiliar", orientacaoFamiliar);
                    cmd.Parameters.AddWithValue("@orientacaoOutro", orientacaoOutro);
                    cmd.Parameters.AddWithValue("@tipoOriCurativos", tipoOriCurativos);
                    cmd.Parameters.AddWithValue("@tipoOriPrevUlcera", tipoOriPrevUlcera);
                    cmd.Parameters.AddWithValue("@tipoOriSondas", tipoOriSondas);
                    cmd.Parameters.AddWithValue("@tipoOriAspiracao", tipoOriAspiracao);
                    cmd.Parameters.AddWithValue("@tipoOriDieta", tipoOriDieta);
                    cmd.Parameters.AddWithValue("@tipoOriPele", tipoOriPele);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // UPDATE PTA
                    string sqlUpdate = @"UPDATE pta_enfermagem SET
                medico=@medico, crm=@crm, hd=@hd, glasgow=@glasgow, descricao=@descricao, dados_iniciais=@iniciais, dados_internacao=@internacaoDados, dados_alta=@dadosAlta, portador_diabetes=@portadorDiabetes, portador_has=@portadorHAS, portador_clinico=@portadorClinico, portador_cirurgico=@portadorCirurgico, portador_outros=@portadorOutros, necessita_ulcera=@necessitaUlcera, necessita_estomas=@necessitaEstomas, necessita_sonda=@necessitaSonda, necessita_traqueo=@necessitaTraqueo, necessita_oxigenio=@necessitaOxigenio, necessita_aspiracao=@necessitaAspiracao, necessita_curativos=@necessitaCurativos, necessita_outros=@necessitaOutros, orientacao_paciente=@orientacaoPaciente, orientacao_familiar=@orientacaoFamiliar, orientacao_outro=@orientacaoOutro, tipo_ori_curativos=@tipoOriCurativos, tipo_ori_prevencao_ulcera=@tipoOriPrevUlcera, tipo_ori_sondas=@tipoOriSondas, tipo_ori_aspiracao=@tipoOriAspiracao, tipo_ori_dieta=@tipoOriDieta, tipo_ori_pele=@tipoOriPele WHERE id_pta_enfermagem=@idPta";

                    SqlCommand cmd = new SqlCommand(sqlUpdate, con);

                    cmd.Parameters.AddWithValue("@medico", medico);
                    cmd.Parameters.AddWithValue("@crm", crm);
                    cmd.Parameters.AddWithValue("@hd", hd);
                    cmd.Parameters.AddWithValue("@glasgow", glasgow);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@iniciais", iniciais);
                    cmd.Parameters.AddWithValue("@internacaoDados", internacaoDados);
                    cmd.Parameters.AddWithValue("@dadosAlta", dadosAlta);
                    cmd.Parameters.AddWithValue("@portadorDiabetes", portadorDiabetes);
                    cmd.Parameters.AddWithValue("@portadorHAS", portadorHAS);
                    cmd.Parameters.AddWithValue("@portadorClinico", portadorClinico);
                    cmd.Parameters.AddWithValue("@portadorCirurgico", portadorCirurgico);
                    cmd.Parameters.AddWithValue("@portadorOutros", portadorOutros);
                    cmd.Parameters.AddWithValue("@necessitaUlcera", necessitaUlcera);
                    cmd.Parameters.AddWithValue("@necessitaEstomas", necessitaEstomas);
                    cmd.Parameters.AddWithValue("@necessitaSonda", necessitaSonda);
                    cmd.Parameters.AddWithValue("@necessitaTraqueo", necessitaTraqueo);
                    cmd.Parameters.AddWithValue("@necessitaOxigenio", necessitaOxigenio);
                    cmd.Parameters.AddWithValue("@necessitaAspiracao", necessitaAspiracao);
                    cmd.Parameters.AddWithValue("@necessitaCurativos", necessitaCurativos);
                    cmd.Parameters.AddWithValue("@necessitaOutros", necessitaOutros);
                    cmd.Parameters.AddWithValue("@orientacaoPaciente", orientacaoPaciente);
                    cmd.Parameters.AddWithValue("@orientacaoFamiliar", orientacaoFamiliar);
                    cmd.Parameters.AddWithValue("@orientacaoOutro", orientacaoOutro);
                    cmd.Parameters.AddWithValue("@tipoOriCurativos", tipoOriCurativos);
                    cmd.Parameters.AddWithValue("@tipoOriPrevUlcera", tipoOriPrevUlcera);
                    cmd.Parameters.AddWithValue("@tipoOriSondas", tipoOriSondas);
                    cmd.Parameters.AddWithValue("@tipoOriAspiracao", tipoOriAspiracao);
                    cmd.Parameters.AddWithValue("@tipoOriDieta", tipoOriDieta);
                    cmd.Parameters.AddWithValue("@tipoOriPele", tipoOriPele);
                    cmd.Parameters.AddWithValue("@idPta", idPta);

                    cmd.ExecuteNonQuery();
                }

                // Recarrega a lista de PTAs depois de salvar/editar
                CarregarListaPTAs(Convert.ToInt32(Session["ID_PACIENTE"]));
            }
        }


        protected void gvPtas_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            int idPTA = (int)gvPtas.SelectedDataKey.Value;

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
            SELECT pta.*, i.setor, i.leito, i.data_internacao, i.data_alta
            FROM pta_enfermagem pta
            INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
            WHERE pta.id_pta_enfermagem = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPTA);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    // Dados da internação vinculada
                    txtSetor.Text = dr["setor"].ToString();
                    txtLeito.Text = dr["leito"].ToString();
                    txtInternacao.Text = Convert.ToDateTime(dr["data_internacao"]).ToString("yyyy-MM-dd");
                    txtAlta.Text = dr["data_alta"] != DBNull.Value
                        ? Convert.ToDateTime(dr["data_alta"]).ToString("yyyy-MM-dd")
                        : "";

                    // PTA Enfermagem
                    txtMedico.Text = dr["medico"].ToString();
                    txtCRM.Text = dr["crm"].ToString();
                    txtHD.Text = dr["hd"].ToString();
                    txtGlasgow.Text = dr["glasgow"].ToString();
                    txtDescricao.Text = dr["descricao"].ToString();
                    txtIniciais.Text = dr["dados_iniciais"].ToString();
                    txtInternacaoDados.Text = dr["dados_internacao"].ToString();
                    txtDadosAlta.Text = dr["dados_alta"].ToString();
                    txtOutros.Text = dr["portador_outros"].ToString();
                    txtOutrosAlta.Text = dr["necessita_outros"].ToString();

                    chkDiabetes.Checked = dr["portador_diabetes"] as bool? ?? false;
                    chkHAS.Checked = dr["portador_has"] as bool? ?? false;
                    chkClinico.Checked = dr["portador_clinico"] as bool? ?? false;
                    chkCirurgico.Checked = dr["portador_cirurgico"] as bool? ?? false;

                    chkUlcera.Checked = dr["necessita_ulcera"] as bool? ?? false;
                    chkEstomas.Checked = dr["necessita_estomas"] as bool? ?? false;
                    chkSonda.Checked = dr["necessita_sonda"] as bool? ?? false;
                    chkTraqueo.Checked = dr["necessita_traqueo"] as bool? ?? false;
                    chkOxigenio.Checked = dr["necessita_oxigenio"] as bool? ?? false;
                    chkAspiracao.Checked = dr["necessita_aspiracao"] as bool? ?? false;
                    chkCurativos.Checked = dr["necessita_curativos"] as bool? ?? false;

                    chkPaciente.Checked = dr["orientacao_paciente"] as bool? ?? false;
                    chkFamiliar.Checked = dr["orientacao_familiar"] as bool? ?? false;
                    chkOutro.Checked = dr["orientacao_outro"] as bool? ?? false;

                    chkCurativosOri.Checked = dr["tipo_ori_curativos"] as bool? ?? false;
                    chkPrevencaoUlcera.Checked = dr["tipo_ori_prevencao_ulcera"] as bool? ?? false;
                    chkSondas.Checked = dr["tipo_ori_sondas"] as bool? ?? false;
                    chkAspiracaoOri.Checked = dr["tipo_ori_aspiracao"] as bool? ?? false;
                    chkDieta.Checked = dr["tipo_ori_dieta"] as bool? ?? false;
                    chkPele.Checked = dr["tipo_ori_pele"] as bool? ?? false;

                    // DESABILITA campos se internação já tem alta!
                    bool somenteLeitura = dr["data_alta"] != DBNull.Value;
                    txtMedico.ReadOnly = somenteLeitura;
                    txtCRM.ReadOnly = somenteLeitura;
                    txtHD.ReadOnly = somenteLeitura;
                    txtGlasgow.ReadOnly = somenteLeitura;
                    txtDescricao.ReadOnly = somenteLeitura;
                    txtIniciais.ReadOnly = somenteLeitura;
                    txtInternacaoDados.ReadOnly = somenteLeitura;
                    txtDadosAlta.ReadOnly = somenteLeitura;
                    txtOutros.ReadOnly = somenteLeitura;
                    txtOutrosAlta.ReadOnly = somenteLeitura;

                    chkDiabetes.Enabled = !somenteLeitura;
                    chkHAS.Enabled = !somenteLeitura;
                    chkClinico.Enabled = !somenteLeitura;
                    chkCirurgico.Enabled = !somenteLeitura;

                    chkUlcera.Enabled = !somenteLeitura;
                    chkEstomas.Enabled = !somenteLeitura;
                    chkSonda.Enabled = !somenteLeitura;
                    chkTraqueo.Enabled = !somenteLeitura;
                    chkOxigenio.Enabled = !somenteLeitura;
                    chkAspiracao.Enabled = !somenteLeitura;
                    chkCurativos.Enabled = !somenteLeitura;

                    chkPaciente.Enabled = !somenteLeitura;
                    chkFamiliar.Enabled = !somenteLeitura;
                    chkOutro.Enabled = !somenteLeitura;

                    chkCurativosOri.Enabled = !somenteLeitura;
                    chkPrevencaoUlcera.Enabled = !somenteLeitura;
                    chkSondas.Enabled = !somenteLeitura;
                    chkAspiracaoOri.Enabled = !somenteLeitura;
                    chkDieta.Enabled = !somenteLeitura;
                    chkPele.Enabled = !somenteLeitura;

                    btnSalvar.Enabled = !somenteLeitura;  

                }
                dr.Close();
            }
        }

    }
}

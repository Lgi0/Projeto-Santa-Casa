using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProjetoIntegrador
{
    public partial class pta_nutricao : System.Web.UI.Page
    {
        string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            int idPaciente = -1;
            if (Request.QueryString["id"] != null)
            {
                idPaciente = Convert.ToInt32(Request.QueryString["id"]);
                Session["ID_PACIENTE"] = idPaciente;
            }
            else if (Session["ID_PACIENTE"] != null)
            {
                idPaciente = (int)Session["ID_PACIENTE"];
            }
            else
            {
                Response.Redirect("Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CarregarPaciente(idPaciente);
                CarregarPTANutricaoAtual(idPaciente);
                CarregarHistoricoPTANutricao(idPaciente);
            }
        }

        private void CarregarPaciente(int id)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = "SELECT nome, nascimento FROM paciente WHERE id_paciente=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtNome.Text = dr["nome"].ToString();
                    DateTime nascimento = Convert.ToDateTime(dr["nascimento"]);
                    int idade = DateTime.Now.Year - nascimento.Year;
                    if (DateTime.Now.DayOfYear < nascimento.DayOfYear) idade--;
                    txtIdade.Text = idade.ToString();
                }
            }
        }

        private void CarregarHistoricoPTANutricao(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                    SELECT 
                        pta.id_pta_nutricao,
                        i.setor,
                        i.leito,
                        i.data_internacao,
                        CASE WHEN i.data_alta IS NULL THEN 'EDITÁVEL' ELSE 'LIBERADO' END as status
                    FROM pta_nutricao pta
                    INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
                    WHERE i.id_paciente = @id
                    ORDER BY i.data_internacao DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                var dt = new System.Data.DataTable();
                dt.Load(cmd.ExecuteReader());
                gvPtaNutricao.DataSource = dt;
                gvPtaNutricao.DataBind();
            }
        }

        protected void gvPtaNutricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idPTA = (int)gvPtaNutricao.SelectedDataKey.Value;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                    SELECT pta.*, i.setor, i.leito, i.data_internacao, i.data_alta
                    FROM pta_nutricao pta
                    INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
                    WHERE pta.id_pta_nutricao = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPTA);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtSetor.Text = dr["setor"].ToString();
                    txtLeito.Text = dr["leito"].ToString();
                    txtInternacao.Text = Convert.ToDateTime(dr["data_internacao"]).ToString("yyyy-MM-dd");
                    txtAlta.Text = dr["data_alta"] != DBNull.Value ? Convert.ToDateTime(dr["data_alta"]).ToString("yyyy-MM-dd") : "";
                    txtMedico.Text = dr["medico"].ToString();
                    txtCRM.Text = dr["crm"].ToString();
                    txtHD.Text = dr["hd"].ToString();
                    txtDescricao.Text = dr["descricao"].ToString();
                    txtIniciais.Text = dr["dados_iniciais"].ToString();
                    txtInternacaoDados.Text = dr["dados_internacao"].ToString();
                    txtDadosAlta.Text = dr["dados_alta"].ToString();
                    txtOutros.Text = dr["portador_outros"].ToString();
                    chkDiabetes.Checked = dr["portador_diabetes"] != DBNull.Value && Convert.ToBoolean(dr["portador_diabetes"]);
                    chkHAS.Checked = dr["portador_has"] != DBNull.Value && Convert.ToBoolean(dr["portador_has"]);
                    chkClinico.Checked = dr["portador_clinico"] != DBNull.Value && Convert.ToBoolean(dr["portador_clinico"]);
                    chkCirurgico.Checked = dr["portador_cirurgico"] != DBNull.Value && Convert.ToBoolean(dr["portador_cirurgico"]);
                    chkInapetencia.Checked = dr["inapetencia"] != DBNull.Value && Convert.ToBoolean(dr["inapetencia"]);
                    chkDesnutricao.Checked = dr["desnutricao"] != DBNull.Value && Convert.ToBoolean(dr["desnutricao"]);
                    chkRisco.Checked = dr["risco_desnutricao"] != DBNull.Value && Convert.ToBoolean(dr["risco_desnutricao"]);
                    chkSobrepeso.Checked = dr["sobrepeso"] != DBNull.Value && Convert.ToBoolean(dr["sobrepeso"]);
                    chkObesidade.Checked = dr["obesidade"] != DBNull.Value && Convert.ToBoolean(dr["obesidade"]);
                    chkNPT.Checked = dr["npt"] != DBNull.Value && Convert.ToBoolean(dr["npt"]);
                    txtOutrosIndicadores.Text = dr["outros_indicadores"]?.ToString();
                    chkOral.Checked = dr["oral"] != DBNull.Value && Convert.ToBoolean(dr["oral"]);
                    chkOralAssistida.Checked = dr["oral_assistida"] != DBNull.Value && Convert.ToBoolean(dr["oral_assistida"]);
                    chkEnteral.Checked = dr["enteral"] != DBNull.Value && Convert.ToBoolean(dr["enteral"]);
                    txtOutrosAlta.Text = dr["outros_alta"]?.ToString();
                    chkPaciente.Checked = dr["orientacao_paciente"] != DBNull.Value && Convert.ToBoolean(dr["orientacao_paciente"]);
                    chkFamiliar.Checked = dr["orientacao_familiar"] != DBNull.Value && Convert.ToBoolean(dr["orientacao_familiar"]);
                    txtOrientacoes.Text = dr["orientacao_quais"]?.ToString();
                    chkDietaEnteral.Checked = dr["dieta_enteral"] != DBNull.Value && Convert.ToBoolean(dr["dieta_enteral"]);
                    chkEncaminhamento.Checked = dr["encaminhamento"] != DBNull.Value && Convert.ToBoolean(dr["encaminhamento"]);
                    chkRedeBasica.Checked = dr["rede_basica"] != DBNull.Value && Convert.ToBoolean(dr["rede_basica"]);
                    txtOutrosAcoes.Text = dr["outros_acoes"]?.ToString();
                    bool somenteLeitura = dr["data_alta"] != DBNull.Value;
                    SetEditavel(!somenteLeitura);
                }
                dr.Close();
            }
        }

        private void CarregarPTANutricaoAtual(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sqlInt = "SELECT TOP 1 id_internacao, setor, leito, data_internacao, data_alta FROM Internacoes WHERE id_paciente = @id AND data_alta IS NULL ORDER BY data_internacao DESC";
                SqlCommand cmdInt = new SqlCommand(sqlInt, con);
                cmdInt.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                SqlDataReader drInt = cmdInt.ExecuteReader();
                if (!drInt.Read()) { SetEditavel(false); drInt.Close(); con.Close(); return; }
                ViewState["ID_INTERNACAO"] = drInt["id_internacao"];
                txtSetor.Text = drInt["setor"].ToString();
                txtLeito.Text = drInt["leito"].ToString();
                txtInternacao.Text = Convert.ToDateTime(drInt["data_internacao"]).ToString("yyyy-MM-dd");
                txtAlta.Text = drInt["data_alta"] != DBNull.Value ? Convert.ToDateTime(drInt["data_alta"]).ToString("yyyy-MM-dd") : "";
                int idInternacao = Convert.ToInt32(drInt["id_internacao"]);
                drInt.Close();

                string sqlPta = "SELECT TOP 1 * FROM pta_nutricao WHERE id_internacao=@idInternacao";
                SqlCommand cmdPta = new SqlCommand(sqlPta, con);
                cmdPta.Parameters.AddWithValue("@idInternacao", idInternacao);
                SqlDataReader drPta = cmdPta.ExecuteReader();

                if (drPta.Read())
                {
                    txtMedico.Text = drPta["medico"]?.ToString();
                    txtCRM.Text = drPta["crm"]?.ToString();
                    txtHD.Text = drPta["hd"]?.ToString();
                    txtDescricao.Text = drPta["descricao"]?.ToString();
                    txtIniciais.Text = drPta["dados_iniciais"]?.ToString();
                    txtInternacaoDados.Text = drPta["dados_internacao"]?.ToString();
                    txtDadosAlta.Text = drPta["dados_alta"]?.ToString();
                    txtOutros.Text = drPta["portador_outros"]?.ToString();
                    chkDiabetes.Checked = drPta["portador_diabetes"] != DBNull.Value && Convert.ToBoolean(drPta["portador_diabetes"]);
                    chkHAS.Checked = drPta["portador_has"] != DBNull.Value && Convert.ToBoolean(drPta["portador_has"]);
                    chkClinico.Checked = drPta["portador_clinico"] != DBNull.Value && Convert.ToBoolean(drPta["portador_clinico"]);
                    chkCirurgico.Checked = drPta["portador_cirurgico"] != DBNull.Value && Convert.ToBoolean(drPta["portador_cirurgico"]);
                    chkInapetencia.Checked = drPta["inapetencia"] != DBNull.Value && Convert.ToBoolean(drPta["inapetencia"]);
                    chkDesnutricao.Checked = drPta["desnutricao"] != DBNull.Value && Convert.ToBoolean(drPta["desnutricao"]);
                    chkRisco.Checked = drPta["risco_desnutricao"] != DBNull.Value && Convert.ToBoolean(drPta["risco_desnutricao"]);
                    chkSobrepeso.Checked = drPta["sobrepeso"] != DBNull.Value && Convert.ToBoolean(drPta["sobrepeso"]);
                    chkObesidade.Checked = drPta["obesidade"] != DBNull.Value && Convert.ToBoolean(drPta["obesidade"]);
                    chkNPT.Checked = drPta["npt"] != DBNull.Value && Convert.ToBoolean(drPta["npt"]);
                    txtOutrosIndicadores.Text = drPta["outros_indicadores"]?.ToString();
                    chkOral.Checked = drPta["oral"] != DBNull.Value && Convert.ToBoolean(drPta["oral"]);
                    chkOralAssistida.Checked = drPta["oral_assistida"] != DBNull.Value && Convert.ToBoolean(drPta["oral_assistida"]);
                    chkEnteral.Checked = drPta["enteral"] != DBNull.Value && Convert.ToBoolean(drPta["enteral"]);
                    txtOutrosAlta.Text = drPta["outros_alta"]?.ToString();
                    chkPaciente.Checked = drPta["orientacao_paciente"] != DBNull.Value && Convert.ToBoolean(drPta["orientacao_paciente"]);
                    chkFamiliar.Checked = drPta["orientacao_familiar"] != DBNull.Value && Convert.ToBoolean(drPta["orientacao_familiar"]);
                    txtOrientacoes.Text = drPta["orientacao_quais"]?.ToString();
                    chkDietaEnteral.Checked = drPta["dieta_enteral"] != DBNull.Value && Convert.ToBoolean(drPta["dieta_enteral"]);
                    chkEncaminhamento.Checked = drPta["encaminhamento"] != DBNull.Value && Convert.ToBoolean(drPta["encaminhamento"]);
                    chkRedeBasica.Checked = drPta["rede_basica"] != DBNull.Value && Convert.ToBoolean(drPta["rede_basica"]);
                    txtOutrosAcoes.Text = drPta["outros_acoes"]?.ToString();

                    SetEditavel(true);
                }
                else
                {
                    SetEditavel(true);
                }
                drPta.Close();
            }
        }

        private void SetEditavel(bool editavel)
        {
            txtMedico.ReadOnly = !editavel;
            txtCRM.ReadOnly = !editavel;
            txtHD.ReadOnly = !editavel;
            txtDescricao.ReadOnly = !editavel;
            txtIniciais.ReadOnly = !editavel;
            txtInternacaoDados.ReadOnly = !editavel;
            txtDadosAlta.ReadOnly = !editavel;
            txtOutros.ReadOnly = !editavel;
            txtOutrosIndicadores.ReadOnly = !editavel;
            txtOutrosAlta.ReadOnly = !editavel;
            txtOrientacoes.ReadOnly = !editavel;
            txtOutrosAcoes.ReadOnly = !editavel;

            chkDiabetes.Enabled = editavel;
            chkHAS.Enabled = editavel;
            chkClinico.Enabled = editavel;
            chkCirurgico.Enabled = editavel;

            chkInapetencia.Enabled = editavel;
            chkDesnutricao.Enabled = editavel;
            chkRisco.Enabled = editavel;
            chkSobrepeso.Enabled = editavel;
            chkObesidade.Enabled = editavel;
            chkNPT.Enabled = editavel;

            chkOral.Enabled = editavel;
            chkOralAssistida.Enabled = editavel;
            chkEnteral.Enabled = editavel;

            chkPaciente.Enabled = editavel;
            chkFamiliar.Enabled = editavel;

            chkDietaEnteral.Enabled = editavel;
            chkEncaminhamento.Enabled = editavel;
            chkRedeBasica.Enabled = editavel;
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
            int idPaciente = (int)Session["ID_PACIENTE"];
            int idInternacao = ViewState["ID_INTERNACAO"] != null ? Convert.ToInt32(ViewState["ID_INTERNACAO"]) : -1;
            if (idInternacao == -1) return;

            string medico = txtMedico.Text.Trim();
            string crm = txtCRM.Text.Trim();
            string hd = txtHD.Text.Trim();
            string descricao = txtDescricao.Text.Trim();
            string iniciais = txtIniciais.Text.Trim();
            string internacaoDados = txtInternacaoDados.Text.Trim();
            string dadosAlta = txtDadosAlta.Text.Trim();
            string portadorOutros = txtOutros.Text.Trim();
            string outrosIndicadores = txtOutrosIndicadores.Text.Trim();
            string outrosAlta = txtOutrosAlta.Text.Trim();
            string orientacaoQuais = txtOrientacoes.Text.Trim();
            string outrosAcoes = txtOutrosAcoes.Text.Trim();

            bool portadorDiabetes = chkDiabetes.Checked;
            bool portadorHAS = chkHAS.Checked;
            bool portadorClinico = chkClinico.Checked;
            bool portadorCirurgico = chkCirurgico.Checked;

            bool inapetencia = chkInapetencia.Checked;
            bool desnutricao = chkDesnutricao.Checked;
            bool riscoDesnutricao = chkRisco.Checked;
            bool sobrepeso = chkSobrepeso.Checked;
            bool obesidade = chkObesidade.Checked;
            bool npt = chkNPT.Checked;

            bool oral = chkOral.Checked;
            bool oralAssistida = chkOralAssistida.Checked;
            bool enteral = chkEnteral.Checked;

            bool orientacaoPaciente = chkPaciente.Checked;
            bool orientacaoFamiliar = chkFamiliar.Checked;

            bool dietaEnteral = chkDietaEnteral.Checked;
            bool encaminhamento = chkEncaminhamento.Checked;
            bool redeBasica = chkRedeBasica.Checked;

            int idPta = -1;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sqlCheck = "SELECT id_pta_nutricao FROM pta_nutricao WHERE id_internacao = @idInternacao";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, con);
                cmdCheck.Parameters.AddWithValue("@idInternacao", idInternacao);
                var result = cmdCheck.ExecuteScalar();
                if (result != null) idPta = Convert.ToInt32(result);

                if (idPta == -1)
                {
                    string sqlInsert = @"INSERT INTO pta_nutricao
                        (id_paciente, id_internacao, medico, crm, hd, descricao, dados_iniciais, dados_internacao, dados_alta, 
                        portador_diabetes, portador_has, portador_clinico, portador_cirurgico, portador_outros,
                        inapetencia, desnutricao, risco_desnutricao, sobrepeso, obesidade, npt, outros_indicadores,
                        oral, oral_assistida, enteral, outros_alta,
                        orientacao_paciente, orientacao_familiar, orientacao_quais,
                        dieta_enteral, encaminhamento, rede_basica, outros_acoes)
                        VALUES
                        (@idPaciente, @idInternacao, @medico, @crm, @hd, @descricao, @iniciais, @internacaoDados, @dadosAlta,
                        @portadorDiabetes, @portadorHAS, @portadorClinico, @portadorCirurgico, @portadorOutros,
                        @inapetencia, @desnutricao, @riscoDesnutricao, @sobrepeso, @obesidade, @npt, @outrosIndicadores,
                        @oral, @oralAssistida, @enteral, @outrosAlta,
                        @orientacaoPaciente, @orientacaoFamiliar, @orientacaoQuais,
                        @dietaEnteral, @encaminhamento, @redeBasica, @outrosAcoes)";

                    SqlCommand cmd = new SqlCommand(sqlInsert, con);
                    cmd.Parameters.AddWithValue("@idPaciente", idPaciente);
                    cmd.Parameters.AddWithValue("@idInternacao", idInternacao);
                    cmd.Parameters.AddWithValue("@medico", medico);
                    cmd.Parameters.AddWithValue("@crm", crm);
                    cmd.Parameters.AddWithValue("@hd", hd);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@iniciais", iniciais);
                    cmd.Parameters.AddWithValue("@internacaoDados", internacaoDados);
                    cmd.Parameters.AddWithValue("@dadosAlta", dadosAlta);
                    cmd.Parameters.AddWithValue("@portadorDiabetes", portadorDiabetes);
                    cmd.Parameters.AddWithValue("@portadorHAS", portadorHAS);
                    cmd.Parameters.AddWithValue("@portadorClinico", portadorClinico);
                    cmd.Parameters.AddWithValue("@portadorCirurgico", portadorCirurgico);
                    cmd.Parameters.AddWithValue("@portadorOutros", portadorOutros);
                    cmd.Parameters.AddWithValue("@inapetencia", inapetencia);
                    cmd.Parameters.AddWithValue("@desnutricao", desnutricao);
                    cmd.Parameters.AddWithValue("@riscoDesnutricao", riscoDesnutricao);
                    cmd.Parameters.AddWithValue("@sobrepeso", sobrepeso);
                    cmd.Parameters.AddWithValue("@obesidade", obesidade);
                    cmd.Parameters.AddWithValue("@npt", npt);
                    cmd.Parameters.AddWithValue("@outrosIndicadores", outrosIndicadores);
                    cmd.Parameters.AddWithValue("@oral", oral);
                    cmd.Parameters.AddWithValue("@oralAssistida", oralAssistida);
                    cmd.Parameters.AddWithValue("@enteral", enteral);
                    cmd.Parameters.AddWithValue("@outrosAlta", outrosAlta);
                    cmd.Parameters.AddWithValue("@orientacaoPaciente", orientacaoPaciente);
                    cmd.Parameters.AddWithValue("@orientacaoFamiliar", orientacaoFamiliar);
                    cmd.Parameters.AddWithValue("@orientacaoQuais", orientacaoQuais);
                    cmd.Parameters.AddWithValue("@dietaEnteral", dietaEnteral);
                    cmd.Parameters.AddWithValue("@encaminhamento", encaminhamento);
                    cmd.Parameters.AddWithValue("@redeBasica", redeBasica);
                    cmd.Parameters.AddWithValue("@outrosAcoes", outrosAcoes);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sqlUpdate = @"UPDATE pta_nutricao SET
                        medico=@medico, crm=@crm, hd=@hd, descricao=@descricao, dados_iniciais=@iniciais, dados_internacao=@internacaoDados,
                        dados_alta=@dadosAlta, portador_diabetes=@portadorDiabetes, portador_has=@portadorHAS,
                        portador_clinico=@portadorClinico, portador_cirurgico=@portadorCirurgico, portador_outros=@portadorOutros,
                        inapetencia=@inapetencia, desnutricao=@desnutricao, risco_desnutricao=@riscoDesnutricao,
                        sobrepeso=@sobrepeso, obesidade=@obesidade, npt=@npt, outros_indicadores=@outrosIndicadores,
                        oral=@oral, oral_assistida=@oralAssistida, enteral=@enteral, outros_alta=@outrosAlta,
                        orientacao_paciente=@orientacaoPaciente, orientacao_familiar=@orientacaoFamiliar,
                        orientacao_quais=@orientacaoQuais, dieta_enteral=@dietaEnteral, encaminhamento=@encaminhamento,
                        rede_basica=@redeBasica, outros_acoes=@outrosAcoes
                        WHERE id_pta_nutricao=@idPta";
                    SqlCommand cmd = new SqlCommand(sqlUpdate, con);

                    cmd.Parameters.AddWithValue("@medico", medico);
                    cmd.Parameters.AddWithValue("@crm", crm);
                    cmd.Parameters.AddWithValue("@hd", hd);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@iniciais", iniciais);
                    cmd.Parameters.AddWithValue("@internacaoDados", internacaoDados);
                    cmd.Parameters.AddWithValue("@dadosAlta", dadosAlta);
                    cmd.Parameters.AddWithValue("@portadorDiabetes", portadorDiabetes);
                    cmd.Parameters.AddWithValue("@portadorHAS", portadorHAS);
                    cmd.Parameters.AddWithValue("@portadorClinico", portadorClinico);
                    cmd.Parameters.AddWithValue("@portadorCirurgico", portadorCirurgico);
                    cmd.Parameters.AddWithValue("@portadorOutros", portadorOutros);
                    cmd.Parameters.AddWithValue("@inapetencia", inapetencia);
                    cmd.Parameters.AddWithValue("@desnutricao", desnutricao);
                    cmd.Parameters.AddWithValue("@riscoDesnutricao", riscoDesnutricao);
                    cmd.Parameters.AddWithValue("@sobrepeso", sobrepeso);
                    cmd.Parameters.AddWithValue("@obesidade", obesidade);
                    cmd.Parameters.AddWithValue("@npt", npt);
                    cmd.Parameters.AddWithValue("@outrosIndicadores", outrosIndicadores);
                    cmd.Parameters.AddWithValue("@oral", oral);
                    cmd.Parameters.AddWithValue("@oralAssistida", oralAssistida);
                    cmd.Parameters.AddWithValue("@enteral", enteral);
                    cmd.Parameters.AddWithValue("@outrosAlta", outrosAlta);
                    cmd.Parameters.AddWithValue("@orientacaoPaciente", orientacaoPaciente);
                    cmd.Parameters.AddWithValue("@orientacaoFamiliar", orientacaoFamiliar);
                    cmd.Parameters.AddWithValue("@orientacaoQuais", orientacaoQuais);
                    cmd.Parameters.AddWithValue("@dietaEnteral", dietaEnteral);
                    cmd.Parameters.AddWithValue("@encaminhamento", encaminhamento);
                    cmd.Parameters.AddWithValue("@redeBasica", redeBasica);
                    cmd.Parameters.AddWithValue("@outrosAcoes", outrosAcoes);
                    cmd.Parameters.AddWithValue("@idPta", idPta);
                    cmd.ExecuteNonQuery();
                }
            }
            CarregarHistoricoPTANutricao(idPaciente);
        }

        protected void btnGerarPDF_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 30, 30, 30, 30);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titlefont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLUE);
                var sectionfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var textfont = FontFactory.GetFont(FontFactory.HELVETICA, 11);

                doc.Add(new Paragraph("Plano Terapêutico para Alta - Nutrição", titlefont));
                doc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 80;
                table.DefaultCell.Padding = 10;
                table.DefaultCell.BorderColor = BaseColor.WHITE;
                table.AddCell(new Phrase("Nome do paciente:", sectionfont));
                table.AddCell(new Phrase(txtNome.Text, textfont));
                table.AddCell(new Phrase("Idade:", sectionfont));
                table.AddCell(new Phrase(txtIdade.Text, textfont));
                table.AddCell(new Phrase("Setor:", sectionfont));
                table.AddCell(new Phrase(txtSetor.Text, textfont));
                table.AddCell(new Phrase("Leito:", sectionfont));
                table.AddCell(new Phrase(txtLeito.Text, textfont));
                table.AddCell(new Phrase("Médico:", sectionfont));
                table.AddCell(new Phrase(txtMedico.Text, textfont));
                table.AddCell(new Phrase("CRM:", sectionfont));
                table.AddCell(new Phrase(txtCRM.Text, textfont));
                table.AddCell(new Phrase("Data da Internação:", sectionfont));
                table.AddCell(new Phrase(txtInternacao.Text, textfont));
                table.AddCell(new Phrase("Data da Alta:", sectionfont));
                table.AddCell(new Phrase(txtAlta.Text, textfont));
                table.AddCell(new Phrase("HD:", sectionfont));
                table.AddCell(new Phrase(txtHD.Text, textfont));
                table.SpacingAfter = 10;
                doc.Add(table);

                doc.Add(new Paragraph("Paciente portador de:", sectionfont));
                doc.Add(new Paragraph($"Diabetes: {(chkDiabetes.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"HAS: {(chkHAS.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Clínico: {(chkClinico.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Cirúrgico: {(chkCirurgico.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutros.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Indicadores de necessidade nutricional:", sectionfont));
                doc.Add(new Paragraph($"Inapetência: {(chkInapetencia.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Desnutrição: {(chkDesnutricao.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Risco de Desnutrição: {(chkRisco.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Sobrepeso: {(chkSobrepeso.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Obesidade: {(chkObesidade.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"NPT: {(chkNPT.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutrosIndicadores.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Alta:", sectionfont));
                doc.Add(new Paragraph($"Oral: {(chkOral.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Oral assistida: {(chkOralAssistida.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Enteral: {(chkEnteral.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutrosAlta.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Orientações realizadas para:", sectionfont));
                doc.Add(new Paragraph($"Paciente: {(chkPaciente.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Familiar/Cuidador: {(chkFamiliar.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Quais: {txtOrientacoes.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Ações desenvolvidas:", sectionfont));
                doc.Add(new Paragraph($"Dieta enteral: {(chkDietaEnteral.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Encaminhamento: {(chkEncaminhamento.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Rede básica: {(chkRedeBasica.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutrosAcoes.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Descrição do caso clínico:", sectionfont));
                doc.Add(new Paragraph(txtDescricao.Text, textfont));
                doc.Add(new Paragraph("Dados Iniciais:", sectionfont));
                doc.Add(new Paragraph(txtIniciais.Text, textfont));
                doc.Add(new Paragraph("Dados da Internação:", sectionfont));
                doc.Add(new Paragraph(txtInternacaoDados.Text, textfont));
                doc.Add(new Paragraph("Dados de Alta:", sectionfont));
                doc.Add(new Paragraph(txtDadosAlta.Text, textfont));

                doc.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=PTA_Nutricao_{txtNome.Text.Replace(" ", "_")}.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }
    }
}

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProjetoIntegrador
{
    public partial class pta_psicologia : System.Web.UI.Page
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
                CarregarPTAPsicologiaAtual(idPaciente);
                CarregarHistoricoPTAPsicologia(idPaciente);
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

        private void CarregarHistoricoPTAPsicologia(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                    SELECT 
                        pta.id_pta_psicologia,
                        i.setor,
                        i.leito,
                        i.data_internacao,
                        CASE WHEN i.data_alta IS NULL THEN 'EDITÁVEL' ELSE 'LIBERADO' END as status
                    FROM pta_psicologia pta
                    INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
                    WHERE i.id_paciente = @id
                    ORDER BY i.data_internacao DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                var dt = new System.Data.DataTable();
                dt.Load(cmd.ExecuteReader());
                gvPtaPsicologia.DataSource = dt;
                gvPtaPsicologia.DataBind();
            }
        }

        protected void gvPtaPsicologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idPTA = (int)gvPtaPsicologia.SelectedDataKey.Value;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                    SELECT pta.*, i.setor, i.leito, i.data_internacao, i.data_alta
                    FROM pta_psicologia pta
                    INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
                    WHERE pta.id_pta_psicologia = @id";
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

                    chkAlteracoes.Checked = dr["alteracoes_psicoemocionais"] != DBNull.Value && Convert.ToBoolean(dr["alteracoes_psicoemocionais"]);
                    chkBaixaAdesao.Checked = dr["baixa_adesao"] != DBNull.Value && Convert.ToBoolean(dr["baixa_adesao"]);
                    chkSuicidio.Checked = dr["suicidio"] != DBNull.Value && Convert.ToBoolean(dr["suicidio"]);
                    chkSuporte.Checked = dr["suporte_emocional"] != DBNull.Value && Convert.ToBoolean(dr["suporte_emocional"]);
                    chkBaixoNivel.Checked = dr["baixo_nivel_comp"] != DBNull.Value && Convert.ToBoolean(dr["baixo_nivel_comp"]);
                    txtOutrosIndicadores.Text = dr["outros_indicadores"]?.ToString();

                    chkIntervencaoPaciente.Checked = dr["intervencao_paciente"] != DBNull.Value && Convert.ToBoolean(dr["intervencao_paciente"]);
                    chkIntervencaoFamiliares.Checked = dr["intervencao_familiares"] != DBNull.Value && Convert.ToBoolean(dr["intervencao_familiares"]);
                    chkOrientacaoFamilia.Checked = dr["orientacao_familia"] != DBNull.Value && Convert.ToBoolean(dr["orientacao_familia"]);
                    txtEncaminhamento.Text = dr["encaminhamento"]?.ToString();

                    bool somenteLeitura = dr["data_alta"] != DBNull.Value;
                    SetEditavel(!somenteLeitura);
                }
                dr.Close();
            }
        }

        private void CarregarPTAPsicologiaAtual(int idPaciente)
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

                string sqlPta = "SELECT TOP 1 * FROM pta_psicologia WHERE id_internacao=@idInternacao";
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
                    chkAlteracoes.Checked = drPta["alteracoes_psicoemocionais"] != DBNull.Value && Convert.ToBoolean(drPta["alteracoes_psicoemocionais"]);
                    chkBaixaAdesao.Checked = drPta["baixa_adesao"] != DBNull.Value && Convert.ToBoolean(drPta["baixa_adesao"]);
                    chkSuicidio.Checked = drPta["suicidio"] != DBNull.Value && Convert.ToBoolean(drPta["suicidio"]);
                    chkSuporte.Checked = drPta["suporte_emocional"] != DBNull.Value && Convert.ToBoolean(drPta["suporte_emocional"]);
                    chkBaixoNivel.Checked = drPta["baixo_nivel_comp"] != DBNull.Value && Convert.ToBoolean(drPta["baixo_nivel_comp"]);
                    txtOutrosIndicadores.Text = drPta["outros_indicadores"]?.ToString();
                    chkIntervencaoPaciente.Checked = drPta["intervencao_paciente"] != DBNull.Value && Convert.ToBoolean(drPta["intervencao_paciente"]);
                    chkIntervencaoFamiliares.Checked = drPta["intervencao_familiares"] != DBNull.Value && Convert.ToBoolean(drPta["intervencao_familiares"]);
                    chkOrientacaoFamilia.Checked = drPta["orientacao_familia"] != DBNull.Value && Convert.ToBoolean(drPta["orientacao_familia"]);
                    txtEncaminhamento.Text = drPta["encaminhamento"]?.ToString();
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
            txtEncaminhamento.ReadOnly = !editavel;

            chkDiabetes.Enabled = editavel;
            chkHAS.Enabled = editavel;
            chkClinico.Enabled = editavel;
            chkCirurgico.Enabled = editavel;

            chkAlteracoes.Enabled = editavel;
            chkBaixaAdesao.Enabled = editavel;
            chkSuicidio.Enabled = editavel;
            chkSuporte.Enabled = editavel;
            chkBaixoNivel.Enabled = editavel;

            chkIntervencaoPaciente.Enabled = editavel;
            chkIntervencaoFamiliares.Enabled = editavel;
            chkOrientacaoFamilia.Enabled = editavel;
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
        protected void btnVoltar_Click(object sender, EventArgs e) { Session.Remove("ID_PACIENTE"); Response.Redirect("Home.aspx"); }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            int idPaciente = (int)Session["ID_PACIENTE"];
            int idInternacao = ViewState["ID_INTERNACAO"] != null ? Convert.ToInt32(ViewState["ID_INTERNACAO"]) : -1;
            if (idInternacao == -1) return;

            string medico = txtMedico.Text.Trim(), crm = txtCRM.Text.Trim(), hd = txtHD.Text.Trim();
            string descricao = txtDescricao.Text.Trim(), iniciais = txtIniciais.Text.Trim();
            string internacaoDados = txtInternacaoDados.Text.Trim(), dadosAlta = txtDadosAlta.Text.Trim();
            string portadorOutros = txtOutros.Text.Trim(), outrosIndicadores = txtOutrosIndicadores.Text.Trim();
            string encaminhamento = txtEncaminhamento.Text.Trim();

            bool portadorDiabetes = chkDiabetes.Checked, portadorHAS = chkHAS.Checked,
                portadorClinico = chkClinico.Checked, portadorCirurgico = chkCirurgico.Checked;

            bool alteracoes = chkAlteracoes.Checked, baixaAdesao = chkBaixaAdesao.Checked, suicidio = chkSuicidio.Checked;
            bool suporte = chkSuporte.Checked, baixoNivel = chkBaixoNivel.Checked;

            bool intervencaoPaciente = chkIntervencaoPaciente.Checked, intervencaoFamiliares = chkIntervencaoFamiliares.Checked;
            bool orientacaoFamilia = chkOrientacaoFamilia.Checked;

            int idPta = -1;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sqlCheck = "SELECT id_pta_psicologia FROM pta_psicologia WHERE id_internacao = @idInternacao";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, con);
                cmdCheck.Parameters.AddWithValue("@idInternacao", idInternacao);
                var result = cmdCheck.ExecuteScalar();
                if (result != null) idPta = Convert.ToInt32(result);

                if (idPta == -1)
                {
                    string sqlInsert = @"INSERT INTO pta_psicologia
                        (id_paciente, id_internacao, medico, crm, hd, descricao, dados_iniciais, dados_internacao, dados_alta, 
                        portador_diabetes, portador_has, portador_clinico, portador_cirurgico, portador_outros,
                        alteracoes_psicoemocionais, baixa_adesao, suicidio, suporte_emocional, baixo_nivel_comp, outros_indicadores,
                        intervencao_paciente, intervencao_familiares, orientacao_familia, encaminhamento)
                        VALUES
                        (@idPaciente, @idInternacao, @medico, @crm, @hd, @descricao, @iniciais, @internacaoDados, @dadosAlta,
                        @portadorDiabetes, @portadorHAS, @portadorClinico, @portadorCirurgico, @portadorOutros,
                        @alteracoes, @baixaAdesao, @suicidio, @suporte, @baixoNivel, @outrosIndicadores,
                        @intervencaoPaciente, @intervencaoFamiliares, @orientacaoFamilia, @encaminhamento)";

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
                    cmd.Parameters.AddWithValue("@alteracoes", alteracoes);
                    cmd.Parameters.AddWithValue("@baixaAdesao", baixaAdesao);
                    cmd.Parameters.AddWithValue("@suicidio", suicidio);
                    cmd.Parameters.AddWithValue("@suporte", suporte);
                    cmd.Parameters.AddWithValue("@baixoNivel", baixoNivel);
                    cmd.Parameters.AddWithValue("@outrosIndicadores", outrosIndicadores);
                    cmd.Parameters.AddWithValue("@intervencaoPaciente", intervencaoPaciente);
                    cmd.Parameters.AddWithValue("@intervencaoFamiliares", intervencaoFamiliares);
                    cmd.Parameters.AddWithValue("@orientacaoFamilia", orientacaoFamilia);
                    cmd.Parameters.AddWithValue("@encaminhamento", encaminhamento);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sqlUpdate = @"UPDATE pta_psicologia SET
                        medico=@medico, crm=@crm, hd=@hd, descricao=@descricao, dados_iniciais=@iniciais, dados_internacao=@internacaoDados,
                        dados_alta=@dadosAlta, portador_diabetes=@portadorDiabetes, portador_has=@portadorHAS,
                        portador_clinico=@portadorClinico, portador_cirurgico=@portadorCirurgico, portador_outros=@portadorOutros,
                        alteracoes_psicoemocionais=@alteracoes, baixa_adesao=@baixaAdesao, suicidio=@suicidio, suporte_emocional=@suporte,
                        baixo_nivel_comp=@baixoNivel, outros_indicadores=@outrosIndicadores,
                        intervencao_paciente=@intervencaoPaciente, intervencao_familiares=@intervencaoFamiliares, 
                        orientacao_familia=@orientacaoFamilia, encaminhamento=@encaminhamento
                        WHERE id_pta_psicologia=@idPta";
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
                    cmd.Parameters.AddWithValue("@alteracoes", alteracoes);
                    cmd.Parameters.AddWithValue("@baixaAdesao", baixaAdesao);
                    cmd.Parameters.AddWithValue("@suicidio", suicidio);
                    cmd.Parameters.AddWithValue("@suporte", suporte);
                    cmd.Parameters.AddWithValue("@baixoNivel", baixoNivel);
                    cmd.Parameters.AddWithValue("@outrosIndicadores", outrosIndicadores);
                    cmd.Parameters.AddWithValue("@intervencaoPaciente", intervencaoPaciente);
                    cmd.Parameters.AddWithValue("@intervencaoFamiliares", intervencaoFamiliares);
                    cmd.Parameters.AddWithValue("@orientacaoFamilia", orientacaoFamilia);
                    cmd.Parameters.AddWithValue("@encaminhamento", encaminhamento);
                    cmd.Parameters.AddWithValue("@idPta", idPta);
                    cmd.ExecuteNonQuery();
                }
            }
            CarregarHistoricoPTAPsicologia(idPaciente);
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

                doc.Add(new Paragraph("Plano Terapêutico para Alta - Psicologia", titlefont));
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

                doc.Add(new Paragraph("Indicadores psicológicos:", sectionfont));
                doc.Add(new Paragraph($"Alterações psicoemocionais: {(chkAlteracoes.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Baixa adesão ao tratamento: {(chkBaixaAdesao.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Tentativa de suicídio: {(chkSuicidio.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Suporte emocional/patologias: {(chkSuporte.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Baixo nível comp: {(chkBaixoNivel.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutrosIndicadores.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Ações desenvolvidas:", sectionfont));
                doc.Add(new Paragraph($"Intervenções c/ paciente: {(chkIntervencaoPaciente.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Intervenções c/ familiares: {(chkIntervencaoFamiliares.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Orientação familiares: {(chkOrientacaoFamilia.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Encaminhamento: {txtEncaminhamento.Text}", textfont));
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
                Response.AddHeader("content-disposition", $"attachment;filename=PTA_Psicologia_{txtNome.Text.Replace(" ", "_")}.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }
    }
}

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProjetoIntegrador
{
    public partial class pta_social : System.Web.UI.Page
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
                CarregarPTASocialAtual(idPaciente);
                CarregarHistoricoPTASocial(idPaciente);
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

        private void CarregarHistoricoPTASocial(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                    SELECT 
                        pta.id_pta_social,
                        i.setor,
                        i.leito,
                        i.data_internacao,
                        CASE WHEN i.data_alta IS NULL THEN 'EDITÁVEL' ELSE 'LIBERADO' END as status
                    FROM pta_social pta
                    INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
                    WHERE i.id_paciente = @id
                    ORDER BY i.data_internacao DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                var dt = new System.Data.DataTable();
                dt.Load(cmd.ExecuteReader());
                gvPtaSocial.DataSource = dt;
                gvPtaSocial.DataBind();
            }
        }

        protected void gvPtaSocial_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idPTA = (int)gvPtaSocial.SelectedDataKey.Value;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                    SELECT pta.*, i.setor, i.leito, i.data_internacao, i.data_alta
                    FROM pta_social pta
                    INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
                    WHERE pta.id_pta_social = @id";
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

                    chkSozinho.Checked = dr["sozinho"] != DBNull.Value && Convert.ToBoolean(dr["sozinho"]);
                    chkConjuge.Checked = dr["conjuge"] != DBNull.Value && Convert.ToBoolean(dr["conjuge"]);
                    chkFilho.Checked = dr["filho"] != DBNull.Value && Convert.ToBoolean(dr["filho"]);
                    chkNoraGenro.Checked = dr["nora_genro"] != DBNull.Value && Convert.ToBoolean(dr["nora_genro"]);
                    chkIrmao.Checked = dr["irmao"] != DBNull.Value && Convert.ToBoolean(dr["irmao"]);
                    chkInstitucionalizado.Checked = dr["institucionalizado"] != DBNull.Value && Convert.ToBoolean(dr["institucionalizado"]);
                    txtOutrosMora.Text = dr["outros_mora"]?.ToString();

                    chkPosSozinho.Checked = dr["pos_sozinho"] != DBNull.Value && Convert.ToBoolean(dr["pos_sozinho"]);
                    chkPosConjuge.Checked = dr["pos_conjuge"] != DBNull.Value && Convert.ToBoolean(dr["pos_conjuge"]);
                    chkPosFilho.Checked = dr["pos_filho"] != DBNull.Value && Convert.ToBoolean(dr["pos_filho"]);
                    chkPosNoraGenro.Checked = dr["pos_nora_genro"] != DBNull.Value && Convert.ToBoolean(dr["pos_nora_genro"]);
                    chkPosIrmao.Checked = dr["pos_irmao"] != DBNull.Value && Convert.ToBoolean(dr["pos_irmao"]);
                    chkPosInstitucionalizado.Checked = dr["pos_institucionalizado"] != DBNull.Value && Convert.ToBoolean(dr["pos_institucionalizado"]);
                    txtOutrosPosAlta.Text = dr["outros_pos_alta"]?.ToString();

                    txtResponsavel.Text = dr["responsavel"]?.ToString();
                    chkAposentadoria.Checked = dr["aposentadoria"] != DBNull.Value && Convert.ToBoolean(dr["aposentadoria"]);
                    chkBolsa.Checked = dr["bolsa_familia"] != DBNull.Value && Convert.ToBoolean(dr["bolsa_familia"]);
                    chkPensao.Checked = dr["pensao"] != DBNull.Value && Convert.ToBoolean(dr["pensao"]);
                    chkBPC.Checked = dr["bpc"] != DBNull.Value && Convert.ToBoolean(dr["bpc"]);
                    chkSalario.Checked = dr["salario"] != DBNull.Value && Convert.ToBoolean(dr["salario"]);
                    txtOutrosRenda.Text = dr["outros_renda"]?.ToString();

                    chkDireitos.Checked = dr["direitos"] != DBNull.Value && Convert.ToBoolean(dr["direitos"]);
                    chkNegligencia.Checked = dr["negligencia"] != DBNull.Value && Convert.ToBoolean(dr["negligencia"]);
                    chkEncaminhamento.Checked = dr["encaminhamento_rede"] != DBNull.Value && Convert.ToBoolean(dr["encaminhamento_rede"]);
                    txtEncaminhamento.Text = dr["encaminhamento"]?.ToString();

                    SetEditavel(dr["data_alta"] == DBNull.Value);
                }
                dr.Close();
            }
        }

        private void CarregarPTASocialAtual(int idPaciente)
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

                string sqlPta = "SELECT TOP 1 * FROM pta_social WHERE id_internacao=@idInternacao";
                SqlCommand cmdPta = new SqlCommand(sqlPta, con);
                cmdPta.Parameters.AddWithValue("@idInternacao", idInternacao);
                SqlDataReader drPta = cmdPta.ExecuteReader();

                if (drPta.Read())
                {
                    // O mesmo preenchimento acima (campos do formulário)
                    // ...
                    SetEditavel(true);
                }
                else SetEditavel(true);
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
            txtOutrosMora.ReadOnly = !editavel;
            txtOutrosPosAlta.ReadOnly = !editavel;
            txtResponsavel.ReadOnly = !editavel;
            txtOutrosRenda.ReadOnly = !editavel;
            txtEncaminhamento.ReadOnly = !editavel;

            chkDiabetes.Enabled = editavel; chkHAS.Enabled = editavel; chkClinico.Enabled = editavel; chkCirurgico.Enabled = editavel;
            chkSozinho.Enabled = editavel; chkConjuge.Enabled = editavel; chkFilho.Enabled = editavel; chkNoraGenro.Enabled = editavel; chkIrmao.Enabled = editavel; chkInstitucionalizado.Enabled = editavel;
            chkPosSozinho.Enabled = editavel; chkPosConjuge.Enabled = editavel; chkPosFilho.Enabled = editavel; chkPosNoraGenro.Enabled = editavel; chkPosIrmao.Enabled = editavel; chkPosInstitucionalizado.Enabled = editavel;
            chkAposentadoria.Enabled = editavel; chkBolsa.Enabled = editavel; chkPensao.Enabled = editavel; chkBPC.Enabled = editavel; chkSalario.Enabled = editavel;
            chkDireitos.Enabled = editavel; chkNegligencia.Enabled = editavel; chkEncaminhamento.Enabled = editavel;
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
            string portadorOutros = txtOutros.Text.Trim(), outrosMora = txtOutrosMora.Text.Trim();
            string outrosPosAlta = txtOutrosPosAlta.Text.Trim(), responsavel = txtResponsavel.Text.Trim();
            string outrosRenda = txtOutrosRenda.Text.Trim(), encaminhamento = txtEncaminhamento.Text.Trim();

            bool portadorDiabetes = chkDiabetes.Checked, portadorHAS = chkHAS.Checked,
                portadorClinico = chkClinico.Checked, portadorCirurgico = chkCirurgico.Checked;
            bool sozinho = chkSozinho.Checked, conjuge = chkConjuge.Checked, filho = chkFilho.Checked,
                 noraGenro = chkNoraGenro.Checked, irmao = chkIrmao.Checked, institucionalizado = chkInstitucionalizado.Checked;
            bool posSozinho = chkPosSozinho.Checked, posConjuge = chkPosConjuge.Checked, posFilho = chkPosFilho.Checked,
                 posNoraGenro = chkPosNoraGenro.Checked, posIrmao = chkPosIrmao.Checked, posInstitucionalizado = chkPosInstitucionalizado.Checked;
            bool aposentadoria = chkAposentadoria.Checked, bolsa = chkBolsa.Checked, pensao = chkPensao.Checked,
                 bpc = chkBPC.Checked, salario = chkSalario.Checked;
            bool direitos = chkDireitos.Checked, negligencia = chkNegligencia.Checked, encaminhamentoRede = chkEncaminhamento.Checked;

            int idPta = -1;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sqlCheck = "SELECT id_pta_social FROM pta_social WHERE id_internacao = @idInternacao";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, con);
                cmdCheck.Parameters.AddWithValue("@idInternacao", idInternacao);
                var result = cmdCheck.ExecuteScalar();
                if (result != null) idPta = Convert.ToInt32(result);

                if (idPta == -1)
                {
                    string sqlInsert = @"INSERT INTO pta_social
                    (id_paciente, id_internacao, medico, crm, hd, descricao, dados_iniciais, dados_internacao, dados_alta, 
                    portador_diabetes, portador_has, portador_clinico, portador_cirurgico, portador_outros,
                    sozinho, conjuge, filho, nora_genro, irmao, institucionalizado, outros_mora,
                    pos_sozinho, pos_conjuge, pos_filho, pos_nora_genro, pos_irmao, pos_institucionalizado, outros_pos_alta,
                    responsavel, aposentadoria, bolsa_familia, pensao, bpc, salario, outros_renda,
                    direitos, negligencia, encaminhamento_rede, encaminhamento)
                    VALUES
                    (@idPaciente, @idInternacao, @medico, @crm, @hd, @descricao, @iniciais, @internacaoDados, @dadosAlta,
                    @portadorDiabetes, @portadorHAS, @portadorClinico, @portadorCirurgico, @portadorOutros,
                    @sozinho, @conjuge, @filho, @noraGenro, @irmao, @institucionalizado, @outrosMora,
                    @posSozinho, @posConjuge, @posFilho, @posNoraGenro, @posIrmao, @posInstitucionalizado, @outrosPosAlta,
                    @responsavel, @aposentadoria, @bolsa, @pensao, @bpc, @salario, @outrosRenda,
                    @direitos, @negligencia, @encaminhamentoRede, @encaminhamento)";
                    SqlCommand cmd = new SqlCommand(sqlInsert, con);
                    cmd.Parameters.AddWithValue("@idPaciente", idPaciente); cmd.Parameters.AddWithValue("@idInternacao", idInternacao);
                    cmd.Parameters.AddWithValue("@medico", medico); cmd.Parameters.AddWithValue("@crm", crm); cmd.Parameters.AddWithValue("@hd", hd);
                    cmd.Parameters.AddWithValue("@descricao", descricao); cmd.Parameters.AddWithValue("@iniciais", iniciais); cmd.Parameters.AddWithValue("@internacaoDados", internacaoDados); cmd.Parameters.AddWithValue("@dadosAlta", dadosAlta);
                    cmd.Parameters.AddWithValue("@portadorDiabetes", portadorDiabetes); cmd.Parameters.AddWithValue("@portadorHAS", portadorHAS);
                    cmd.Parameters.AddWithValue("@portadorClinico", portadorClinico); cmd.Parameters.AddWithValue("@portadorCirurgico", portadorCirurgico); cmd.Parameters.AddWithValue("@portadorOutros", portadorOutros);
                    cmd.Parameters.AddWithValue("@sozinho", sozinho); cmd.Parameters.AddWithValue("@conjuge", conjuge); cmd.Parameters.AddWithValue("@filho", filho); cmd.Parameters.AddWithValue("@noraGenro", noraGenro); cmd.Parameters.AddWithValue("@irmao", irmao); cmd.Parameters.AddWithValue("@institucionalizado", institucionalizado); cmd.Parameters.AddWithValue("@outrosMora", outrosMora);
                    cmd.Parameters.AddWithValue("@posSozinho", posSozinho); cmd.Parameters.AddWithValue("@posConjuge", posConjuge); cmd.Parameters.AddWithValue("@posFilho", posFilho); cmd.Parameters.AddWithValue("@posNoraGenro", posNoraGenro); cmd.Parameters.AddWithValue("@posIrmao", posIrmao); cmd.Parameters.AddWithValue("@posInstitucionalizado", posInstitucionalizado); cmd.Parameters.AddWithValue("@outrosPosAlta", outrosPosAlta);
                    cmd.Parameters.AddWithValue("@responsavel", responsavel); cmd.Parameters.AddWithValue("@aposentadoria", aposentadoria); cmd.Parameters.AddWithValue("@bolsa", bolsa); cmd.Parameters.AddWithValue("@pensao", pensao); cmd.Parameters.AddWithValue("@bpc", bpc); cmd.Parameters.AddWithValue("@salario", salario); cmd.Parameters.AddWithValue("@outrosRenda", outrosRenda);
                    cmd.Parameters.AddWithValue("@direitos", direitos); cmd.Parameters.AddWithValue("@negligencia", negligencia); cmd.Parameters.AddWithValue("@encaminhamentoRede", encaminhamentoRede); cmd.Parameters.AddWithValue("@encaminhamento", encaminhamento);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sqlUpdate = @"UPDATE pta_social SET
                    medico=@medico, crm=@crm, hd=@hd, descricao=@descricao, dados_iniciais=@iniciais, dados_internacao=@internacaoDados, dados_alta=@dadosAlta,
                    portador_diabetes=@portadorDiabetes, portador_has=@portadorHAS, portador_clinico=@portadorClinico, portador_cirurgico=@portadorCirurgico, portador_outros=@portadorOutros,
                    sozinho=@sozinho, conjuge=@conjuge, filho=@filho, nora_genro=@noraGenro, irmao=@irmao, institucionalizado=@institucionalizado, outros_mora=@outrosMora,
                    pos_sozinho=@posSozinho, pos_conjuge=@posConjuge, pos_filho=@posFilho, pos_nora_genro=@posNoraGenro, pos_irmao=@posIrmao, pos_institucionalizado=@posInstitucionalizado, outros_pos_alta=@outrosPosAlta,
                    responsavel=@responsavel, aposentadoria=@aposentadoria, bolsa_familia=@bolsa, pensao=@pensao, bpc=@bpc, salario=@salario, outros_renda=@outrosRenda,
                    direitos=@direitos, negligencia=@negligencia, encaminhamento_rede=@encaminhamentoRede, encaminhamento=@encaminhamento
                    WHERE id_pta_social=@idPta";
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
                    cmd.Parameters.AddWithValue("@sozinho", sozinho);
                    cmd.Parameters.AddWithValue("@conjuge", conjuge);
                    cmd.Parameters.AddWithValue("@filho", filho);
                    cmd.Parameters.AddWithValue("@noraGenro", noraGenro);
                    cmd.Parameters.AddWithValue("@irmao", irmao);
                    cmd.Parameters.AddWithValue("@institucionalizado", institucionalizado);
                    cmd.Parameters.AddWithValue("@outrosMora", outrosMora);
                    cmd.Parameters.AddWithValue("@posSozinho", posSozinho);
                    cmd.Parameters.AddWithValue("@posConjuge", posConjuge);
                    cmd.Parameters.AddWithValue("@posFilho", posFilho);
                    cmd.Parameters.AddWithValue("@posNoraGenro", posNoraGenro);
                    cmd.Parameters.AddWithValue("@posIrmao", posIrmao);
                    cmd.Parameters.AddWithValue("@posInstitucionalizado", posInstitucionalizado);
                    cmd.Parameters.AddWithValue("@outrosPosAlta", outrosPosAlta);
                    cmd.Parameters.AddWithValue("@responsavel", responsavel);
                    cmd.Parameters.AddWithValue("@aposentadoria", aposentadoria);
                    cmd.Parameters.AddWithValue("@bolsa", bolsa);
                    cmd.Parameters.AddWithValue("@pensao", pensao);
                    cmd.Parameters.AddWithValue("@bpc", bpc);
                    cmd.Parameters.AddWithValue("@salario", salario);
                    cmd.Parameters.AddWithValue("@outrosRenda", outrosRenda);
                    cmd.Parameters.AddWithValue("@direitos", direitos);
                    cmd.Parameters.AddWithValue("@negligencia", negligencia);
                    cmd.Parameters.AddWithValue("@encaminhamentoRede", encaminhamentoRede);
                    cmd.Parameters.AddWithValue("@encaminhamento", encaminhamento);
                    cmd.Parameters.AddWithValue("@idPta", idPta);
                    cmd.ExecuteNonQuery();
                }
            }
            CarregarHistoricoPTASocial(idPaciente);
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

                doc.Add(new Paragraph("Plano Terapêutico para Alta - Serviço Social", titlefont));
                doc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 95;
                table.DefaultCell.Padding = 5;
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

                doc.Add(new Paragraph("Mora com:", sectionfont));
                doc.Add(new Paragraph($"Sozinho: {(chkSozinho.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Cônjuge: {(chkConjuge.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Filho(a): {(chkFilho.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Nora/Genro: {(chkNoraGenro.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Irmão(ã): {(chkIrmao.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Institucionalizado: {(chkInstitucionalizado.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutrosMora.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Pós-alta residirá com:", sectionfont));
                doc.Add(new Paragraph($"Sozinho: {(chkPosSozinho.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Cônjuge: {(chkPosConjuge.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Filho(a): {(chkPosFilho.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Nora/Genro: {(chkPosNoraGenro.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Irmão(ã): {(chkPosIrmao.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Institucionalizado: {(chkPosInstitucionalizado.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutrosPosAlta.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Familiar responsável:", sectionfont));
                doc.Add(new Paragraph(txtResponsavel.Text, textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Renda proveniente de:", sectionfont));
                doc.Add(new Paragraph($"Aposentadoria: {(chkAposentadoria.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Bolsa Família: {(chkBolsa.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Pensão: {(chkPensao.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"BPC: {(chkBPC.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Salário: {(chkSalario.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outros: {txtOutrosRenda.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Ações desenvolvidas:", sectionfont));
                doc.Add(new Paragraph($"Direitos/previdência: {(chkDireitos.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Negligência/violência: {(chkNegligencia.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Enc. rede básica saúde: {(chkEncaminhamento.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Encaminhamento para: {txtEncaminhamento.Text}", textfont));
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
                Response.AddHeader("content-disposition", $"attachment;filename=PTA_ServicoSocial_{txtNome.Text.Replace(" ", "_")}.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }
    }
}

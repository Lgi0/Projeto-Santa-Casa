using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace ProjetoIntegrador
{
    public partial class pta_fisioterapia : System.Web.UI.Page
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
                CarregarPTAFisioterapiaAtual(idPaciente);
                CarregarHistoricoPTAFisioterapia(idPaciente);

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

        protected void gvPtaFisio_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idPTA = (int)gvPtaFisio.SelectedDataKey.Value;

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
            SELECT pta.*, i.setor, i.leito, i.data_internacao, i.data_alta
            FROM pta_fisioterapia pta
            INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
            WHERE pta.id_pta_fisioterapia = @id";
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

                    // PTA Fisio (igual ao preencher campos principal!)
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
                    chkMotora.Checked = dr["necessidade_motora"] != DBNull.Value && Convert.ToBoolean(dr["necessidade_motora"]);
                    chkRespiratoria.Checked = dr["necessidade_respiratoria"] != DBNull.Value && Convert.ToBoolean(dr["necessidade_respiratoria"]);
                    chkDeambulacao.Checked = dr["necessidade_deambulacao"] != DBNull.Value && Convert.ToBoolean(dr["necessidade_deambulacao"]);
                    chkProdutiva.Checked = dr["tosse_produtiva"] != DBNull.Value && Convert.ToBoolean(dr["tosse_produtiva"]);
                    chkImprodutiva.Checked = dr["tosse_improdutiva"] != DBNull.Value && Convert.ToBoolean(dr["tosse_improdutiva"]);
                    chkAspiracao.Checked = dr["aspiracao_periodica"] != DBNull.Value && Convert.ToBoolean(dr["aspiracao_periodica"]);

                    txtBanhar.Text = dr["banhar_se"]?.ToString();
                    txtVestir.Text = dr["vestir_se"]?.ToString();
                    txtBanheiro.Text = dr["ir_banheiro"]?.ToString();
                    txtTransferencia.Text = dr["transferencia"]?.ToString();
                    txtContinencia.Text = dr["continencia"]?.ToString();
                    txtAlimentacao.Text = dr["alimentacao"]?.ToString();
                    txtTotal.Text = dr["total_dependencia"]?.ToString();

                    chkAltaMotora.Checked = dr["alta_motora"] != DBNull.Value && Convert.ToBoolean(dr["alta_motora"]);
                    chkAltaRespiratoria.Checked = dr["alta_respiratoria"] != DBNull.Value && Convert.ToBoolean(dr["alta_respiratoria"]);
                    txtOutroAlta.Text = dr["alta_outro"].ToString();

                    chkEncaminhamentos.Checked = dr["encaminhamentos"] != DBNull.Value && Convert.ToBoolean(dr["encaminhamentos"]);
                    chkOrientacoes.Checked = dr["orientacoes"] != DBNull.Value && Convert.ToBoolean(dr["orientacoes"]);
                }
            }
            }


        private void CarregarHistoricoPTAFisioterapia(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
            SELECT 
                pta.id_pta_fisioterapia,
                i.setor,
                i.leito,
                i.data_internacao,
                i.data_alta,
                CASE 
                    WHEN i.data_alta IS NULL THEN 'EDITÁVEL'
                    ELSE 'LIBERADO'
                END as status
            FROM pta_fisioterapia pta
            INNER JOIN Internacoes i ON pta.id_internacao = i.id_internacao
            WHERE i.id_paciente = @id
            ORDER BY i.data_internacao DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                var dt = new System.Data.DataTable();
                dt.Load(cmd.ExecuteReader());
                gvPtaFisio.DataSource = dt;
                gvPtaFisio.DataBind();
            }
        }


        private void CarregarPTAFisioterapiaAtual(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                // Busca internação aberta
                string sqlInt = "SELECT TOP 1 id_internacao, setor, leito, data_internacao, data_alta FROM Internacoes WHERE id_paciente = @id AND data_alta IS NULL ORDER BY data_internacao DESC";
                SqlCommand cmdInt = new SqlCommand(sqlInt, con);
                cmdInt.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                SqlDataReader drInt = cmdInt.ExecuteReader();

                // Se não existir internação aberta, limpa e trava tudo
                if (!drInt.Read())
                {
                    SetEditavel(false);
                    LimparCampos();
                    drInt.Close();
                    con.Close();
                    return;
                }

                // Preenche dados da internação ativa
                ViewState["ID_INTERNACAO"] = drInt["id_internacao"];
                txtSetor.Text = drInt["setor"].ToString();
                txtLeito.Text = drInt["leito"].ToString();
                txtInternacao.Text = Convert.ToDateTime(drInt["data_internacao"]).ToString("yyyy-MM-dd");
                txtAlta.Text = drInt["data_alta"] != DBNull.Value
                    ? Convert.ToDateTime(drInt["data_alta"]).ToString("yyyy-MM-dd")
                    : "";

                int idInternacao = Convert.ToInt32(drInt["id_internacao"]);
                drInt.Close();

                // Busca PTA Fisioterapia dessa internação ativa
                string sqlPta = "SELECT TOP 1 * FROM pta_fisioterapia WHERE id_internacao=@idInternacao";
                SqlCommand cmdPta = new SqlCommand(sqlPta, con);
                cmdPta.Parameters.AddWithValue("@idInternacao", idInternacao);
                SqlDataReader drPta = cmdPta.ExecuteReader();

                if (drPta.Read())
                {
                    // Preencha os campos conforme sua modelagem!
                    txtMedico.Text = drPta["medico"]?.ToString();
                    txtCRM.Text = drPta["crm"]?.ToString();
                    txtHD.Text = drPta["hd"]?.ToString();
                    txtDescricao.Text = drPta["descricao"]?.ToString();
                    txtIniciais.Text = drPta["dados_iniciais"]?.ToString();
                    txtInternacaoDados.Text = drPta["dados_internacao"]?.ToString();
                    txtDadosAlta.Text = drPta["dados_alta"]?.ToString();

                    chkDiabetes.Checked = drPta["portador_diabetes"] as bool? ?? false;
                    chkHAS.Checked = drPta["portador_has"] as bool? ?? false;
                    chkClinico.Checked = drPta["portador_clinico"] as bool? ?? false;
                    chkCirurgico.Checked = drPta["portador_cirurgico"] as bool? ?? false;
                    txtOutros.Text = drPta["portador_outros"]?.ToString();

                    chkMotora.Checked = drPta["necessidade_motora"] as bool? ?? false;
                    chkRespiratoria.Checked = drPta["necessidade_respiratoria"] as bool? ?? false;
                    chkDeambulacao.Checked = drPta["necessidade_deambulacao"] as bool? ?? false;
                    chkProdutiva.Checked = drPta["tosse_produtiva"] as bool? ?? false;
                    chkImprodutiva.Checked = drPta["tosse_improdutiva"] as bool? ?? false;
                    chkAspiracao.Checked = drPta["aspiracao_periodica"] as bool? ?? false;

                    txtBanhar.Text = drPta["banhar_se"]?.ToString();
                    txtVestir.Text = drPta["vestir_se"]?.ToString();
                    txtBanheiro.Text = drPta["ir_banheiro"]?.ToString();
                    txtTransferencia.Text = drPta["transferencia"]?.ToString();
                    txtContinencia.Text = drPta["continencia"]?.ToString();
                    txtAlimentacao.Text = drPta["alimentacao"]?.ToString();
                    txtTotal.Text = drPta["total_dependencia"]?.ToString();

                    chkAltaMotora.Checked = drPta["alta_motora"] as bool? ?? false;
                    chkAltaRespiratoria.Checked = drPta["alta_respiratoria"] as bool? ?? false;
                    txtOutroAlta.Text = drPta["alta_outro"]?.ToString();

                    chkEncaminhamentos.Checked = drPta["encaminhamentos"] as bool? ?? false;
                    chkOrientacoes.Checked = drPta["orientacoes"] as bool? ?? false;
                    txtQuais.Text = drPta["quais_encaminhamentos"]?.ToString();

                    SetEditavel(true);
                }
                else
                {
                    LimparCampos();
                    SetEditavel(true);
                }
                drPta.Close();
            }
        }

        private void SetEditavel(bool editavel)
        {
            // Trava/destrava todos os campos principais do form
            txtMedico.ReadOnly = !editavel;
            txtCRM.ReadOnly = !editavel;
            txtHD.ReadOnly = !editavel;
            txtDescricao.ReadOnly = !editavel;
            txtIniciais.ReadOnly = !editavel;
            txtInternacaoDados.ReadOnly = !editavel;
            txtDadosAlta.ReadOnly = !editavel;
            txtOutros.ReadOnly = !editavel;

            chkDiabetes.Enabled = editavel;
            chkHAS.Enabled = editavel;
            chkClinico.Enabled = editavel;
            chkCirurgico.Enabled = editavel;
            chkMotora.Enabled = editavel;
            chkRespiratoria.Enabled = editavel;
            chkDeambulacao.Enabled = editavel;
            chkProdutiva.Enabled = editavel;
            chkImprodutiva.Enabled = editavel;
            chkAspiracao.Enabled = editavel;

            txtBanhar.ReadOnly = !editavel;
            txtVestir.ReadOnly = !editavel;
            txtBanheiro.ReadOnly = !editavel;
            txtTransferencia.ReadOnly = !editavel;
            txtContinencia.ReadOnly = !editavel;
            txtAlimentacao.ReadOnly = !editavel;
            txtTotal.ReadOnly = !editavel;

            chkAltaMotora.Enabled = editavel;
            chkAltaRespiratoria.Enabled = editavel;
            txtOutroAlta.ReadOnly = !editavel;

            chkEncaminhamentos.Enabled = editavel;
            chkOrientacoes.Enabled = editavel;
            txtQuais.ReadOnly = !editavel;
        }

        private void LimparCampos()
        {
            txtMedico.Text = "";
            txtCRM.Text = "";
            txtHD.Text = "";
            txtDescricao.Text = "";
            txtIniciais.Text = "";
            txtInternacaoDados.Text = "";
            txtDadosAlta.Text = "";
            txtOutros.Text = "";
            chkDiabetes.Checked = false; chkHAS.Checked = false; chkClinico.Checked = false; chkCirurgico.Checked = false;
            chkMotora.Checked = false; chkRespiratoria.Checked = false; chkDeambulacao.Checked = false;
            chkProdutiva.Checked = false; chkImprodutiva.Checked = false; chkAspiracao.Checked = false;
            txtBanhar.Text = ""; txtVestir.Text = ""; txtBanheiro.Text = ""; txtTransferencia.Text = ""; txtContinencia.Text = ""; txtAlimentacao.Text = ""; txtTotal.Text = "";
            chkAltaMotora.Checked = false; chkAltaRespiratoria.Checked = false; txtOutroAlta.Text = "";
            chkEncaminhamentos.Checked = false; chkOrientacoes.Checked = false; txtQuais.Text = "";
        }

        // Implemente métodos similares para btnSalvar_Click e btnGerarPDF_Click (usando os campos da fisio)
        // Se quiser, peço para te gerar esses métodos também (só pedir!)

        // Demais eventos dos botões de navegação/módulos
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
            if (idInternacao == -1)
            {
                // Garantir que o id está disponível!
                return;
            }

            // Captura os dados do formulário
            string medico = txtMedico.Text.Trim();
            string crm = txtCRM.Text.Trim();
            string hd = txtHD.Text.Trim();
            string descricao = txtDescricao.Text.Trim();
            string iniciais = txtIniciais.Text.Trim();
            string internacaoDados = txtInternacaoDados.Text.Trim();
            string dadosAlta = txtDadosAlta.Text.Trim();
            string portadorOutros = txtOutros.Text.Trim();
            string altaOutro = txtOutroAlta.Text.Trim();
            string quaisEncaminhamentos = txtQuais.Text.Trim();

            // campos booleanos
            bool portadorDiabetes = chkDiabetes.Checked;
            bool portadorHAS = chkHAS.Checked;
            bool portadorClinico = chkClinico.Checked;
            bool portadorCirurgico = chkCirurgico.Checked;

            bool necessidadeMotora = chkMotora.Checked;
            bool necessidadeRespiratoria = chkRespiratoria.Checked;
            bool necessidadeDeambulacao = chkDeambulacao.Checked;
            bool tosseProdutiva = chkProdutiva.Checked;
            bool tosseImprodutiva = chkImprodutiva.Checked;
            bool aspiracaoPeriodica = chkAspiracao.Checked;

            int banhar = string.IsNullOrWhiteSpace(txtBanhar.Text) ? 0 : Convert.ToInt32(txtBanhar.Text);
            int vestir = string.IsNullOrWhiteSpace(txtVestir.Text) ? 0 : Convert.ToInt32(txtVestir.Text);
            int banheiro = string.IsNullOrWhiteSpace(txtBanheiro.Text) ? 0 : Convert.ToInt32(txtBanheiro.Text);
            int transferencia = string.IsNullOrWhiteSpace(txtTransferencia.Text) ? 0 : Convert.ToInt32(txtTransferencia.Text);
            int continencia = string.IsNullOrWhiteSpace(txtContinencia.Text) ? 0 : Convert.ToInt32(txtContinencia.Text);
            int alimentacao = string.IsNullOrWhiteSpace(txtAlimentacao.Text) ? 0 : Convert.ToInt32(txtAlimentacao.Text);
            int total = string.IsNullOrWhiteSpace(txtTotal.Text) ? 0 : Convert.ToInt32(txtTotal.Text);

            bool altaMotora = chkAltaMotora.Checked;
            bool altaRespiratoria = chkAltaRespiratoria.Checked;

            bool encaminhamentos = chkEncaminhamentos.Checked;
            bool orientacoes = chkOrientacoes.Checked;

            int idPta = -1;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                string sqlCheck = "SELECT id_pta_fisioterapia FROM pta_fisioterapia WHERE id_internacao = @idInternacao";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, con);
                cmdCheck.Parameters.AddWithValue("@idInternacao", idInternacao);
                var result = cmdCheck.ExecuteScalar();
                if (result != null)
                {
                    idPta = Convert.ToInt32(result);
                }

                if (idPta == -1)
                {
                    // INSERE NOVA PTA FISIO
                    string sqlInsert = @"INSERT INTO pta_fisioterapia
                (id_paciente, id_internacao, medico, crm, hd, descricao, dados_iniciais, dados_internacao, dados_alta, 
                 portador_diabetes, portador_has, portador_clinico, portador_cirurgico, portador_outros, 
                 necessidade_motora, necessidade_respiratoria, necessidade_deambulacao, tosse_produtiva, tosse_improdutiva, aspiracao_periodica,
                 banhar_se, vestir_se, ir_banheiro, transferencia, continencia, alimentacao, total_dependencia,
                 alta_motora, alta_respiratoria, alta_outro, encaminhamentos, orientacoes, quais_encaminhamentos)
                 VALUES
                (@idPaciente, @idInternacao, @medico, @crm, @hd, @descricao, @iniciais, @internacaoDados, @dadosAlta,
                @portadorDiabetes, @portadorHAS, @portadorClinico, @portadorCirurgico, @portadorOutros,
                @necessidadeMotora, @necessidadeRespiratoria, @necessidadeDeambulacao, @tosseProdutiva, @tosseImprodutiva, @aspiracaoPeriodica,
                @banhar, @vestir, @banheiro, @transferencia, @continencia, @alimentacao, @total,
                @altaMotora, @altaRespiratoria, @altaOutro, @encaminhamentos, @orientacoes, @quaisEncaminhamentos)";
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
                    cmd.Parameters.AddWithValue("@necessidadeMotora", necessidadeMotora);
                    cmd.Parameters.AddWithValue("@necessidadeRespiratoria", necessidadeRespiratoria);
                    cmd.Parameters.AddWithValue("@necessidadeDeambulacao", necessidadeDeambulacao);
                    cmd.Parameters.AddWithValue("@tosseProdutiva", tosseProdutiva);
                    cmd.Parameters.AddWithValue("@tosseImprodutiva", tosseImprodutiva);
                    cmd.Parameters.AddWithValue("@aspiracaoPeriodica", aspiracaoPeriodica);
                    cmd.Parameters.AddWithValue("@banhar", banhar);
                    cmd.Parameters.AddWithValue("@vestir", vestir);
                    cmd.Parameters.AddWithValue("@banheiro", banheiro);
                    cmd.Parameters.AddWithValue("@transferencia", transferencia);
                    cmd.Parameters.AddWithValue("@continencia", continencia);
                    cmd.Parameters.AddWithValue("@alimentacao", alimentacao);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@altaMotora", altaMotora);
                    cmd.Parameters.AddWithValue("@altaRespiratoria", altaRespiratoria);
                    cmd.Parameters.AddWithValue("@altaOutro", altaOutro);
                    cmd.Parameters.AddWithValue("@encaminhamentos", encaminhamentos);
                    cmd.Parameters.AddWithValue("@orientacoes", orientacoes);
                    cmd.Parameters.AddWithValue("@quaisEncaminhamentos", quaisEncaminhamentos);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string sqlUpdate = @"UPDATE pta_fisioterapia SET
                    medico=@medico, crm=@crm, hd=@hd, descricao=@descricao, dados_iniciais=@iniciais, dados_internacao=@internacaoDados, dados_alta=@dadosAlta,
                    portador_diabetes=@portadorDiabetes, portador_has=@portadorHAS, portador_clinico=@portadorClinico, portador_cirurgico=@portadorCirurgico, portador_outros=@portadorOutros,
                    necessidade_motora=@necessidadeMotora, necessidade_respiratoria=@necessidadeRespiratoria, necessidade_deambulacao=@necessidadeDeambulacao, 
                    tosse_produtiva=@tosseProdutiva, tosse_improdutiva=@tosseImprodutiva, aspiracao_periodica=@aspiracaoPeriodica,
                    banhar_se=@banhar, vestir_se=@vestir, ir_banheiro=@banheiro, transferencia=@transferencia, continencia=@continencia, alimentacao=@alimentacao, 
                    total_dependencia=@total,
                    alta_motora=@altaMotora, alta_respiratoria=@altaRespiratoria, alta_outro=@altaOutro,
                    encaminhamentos=@encaminhamentos, orientacoes=@orientacoes, quais_encaminhamentos=@quaisEncaminhamentos
                WHERE id_pta_fisioterapia=@idPta";
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
                    cmd.Parameters.AddWithValue("@necessidadeMotora", necessidadeMotora);
                    cmd.Parameters.AddWithValue("@necessidadeRespiratoria", necessidadeRespiratoria);
                    cmd.Parameters.AddWithValue("@necessidadeDeambulacao", necessidadeDeambulacao);
                    cmd.Parameters.AddWithValue("@tosseProdutiva", tosseProdutiva);
                    cmd.Parameters.AddWithValue("@tosseImprodutiva", tosseImprodutiva);
                    cmd.Parameters.AddWithValue("@aspiracaoPeriodica", aspiracaoPeriodica);
                    cmd.Parameters.AddWithValue("@banhar", banhar);
                    cmd.Parameters.AddWithValue("@vestir", vestir);
                    cmd.Parameters.AddWithValue("@banheiro", banheiro);
                    cmd.Parameters.AddWithValue("@transferencia", transferencia);
                    cmd.Parameters.AddWithValue("@continencia", continencia);
                    cmd.Parameters.AddWithValue("@alimentacao", alimentacao);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@altaMotora", altaMotora);
                    cmd.Parameters.AddWithValue("@altaRespiratoria", altaRespiratoria);
                    cmd.Parameters.AddWithValue("@altaOutro", altaOutro);
                    cmd.Parameters.AddWithValue("@encaminhamentos", encaminhamentos);
                    cmd.Parameters.AddWithValue("@orientacoes", orientacoes);
                    cmd.Parameters.AddWithValue("@quaisEncaminhamentos", quaisEncaminhamentos);
                    cmd.Parameters.AddWithValue("@idPta", idPta);

                    cmd.ExecuteNonQuery();
                }
            }
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

                doc.Add(new Paragraph("Plano Terapêutico para Alta - Fisioterapia", titlefont));
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

                doc.Add(new Paragraph("Necessidade de Fisioterapia:", sectionfont));
                doc.Add(new Paragraph($"Motora: {(chkMotora.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Respiratória: {(chkRespiratoria.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Deambulação precoce: {(chkDeambulacao.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Tosse produtiva: {(chkProdutiva.Checked ? "Sim" : "Não")} | Tosse improdutiva: {(chkImprodutiva.Checked ? "Sim" : "Não")} | Aspiração periódica: {(chkAspiracao.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Níveis de Dependência/Independência:", sectionfont));
                doc.Add(new Paragraph($"Banhar-se: {txtBanhar.Text}", textfont));
                doc.Add(new Paragraph($"Vestir-se: {txtVestir.Text}", textfont));
                doc.Add(new Paragraph($"Ir ao Banheiro: {txtBanheiro.Text}", textfont));
                doc.Add(new Paragraph($"Transferência: {txtTransferencia.Text}", textfont));
                doc.Add(new Paragraph($"Continência: {txtContinencia.Text}", textfont));
                doc.Add(new Paragraph($"Alimentação: {txtAlimentacao.Text}", textfont));
                doc.Add(new Paragraph($"Total: {txtTotal.Text}", textfont));
                doc.Add(new Paragraph("\nAlta:"));
                doc.Add(new Paragraph($"Motora: {(chkAltaMotora.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Respiratória: {(chkAltaRespiratoria.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Outro: {txtOutroAlta.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Ações Realizadas:", sectionfont));
                doc.Add(new Paragraph($"Encaminhamentos: {(chkEncaminhamentos.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Orientações: {(chkOrientacoes.Checked ? "Sim" : "Não")}", textfont));
                doc.Add(new Paragraph($"Quais: {txtQuais.Text}", textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Descrição do caso clínico:", sectionfont));
                doc.Add(new Paragraph(txtDescricao.Text, textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Dados Iniciais:", sectionfont));
                doc.Add(new Paragraph(txtIniciais.Text, textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Dados da Internação:", sectionfont));
                doc.Add(new Paragraph(txtInternacaoDados.Text, textfont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Dados de Alta:", sectionfont));
                doc.Add(new Paragraph(txtDadosAlta.Text, textfont));
                doc.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=PTA_Fisioterapia_{txtNome.Text.Replace(" ", "_")}.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }

    }
}

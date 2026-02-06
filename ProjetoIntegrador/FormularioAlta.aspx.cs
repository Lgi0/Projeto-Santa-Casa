using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoIntegrador
{
    public partial class FormularioAlta : System.Web.UI.Page
    {
        string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
        int idPaciente;

        protected void Page_Load(object sender, EventArgs e)
        {
            string idParam = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out idPaciente))
            {
                lblMsg.Text = "ID do paciente não foi passado ou é inválido.";
                return;
            }
            CarregarDadosPaciente(idPaciente);

            if (!IsPostBack)
            {
                CarregarInternacoes(idPaciente, null);
                LimparFormulario();
            }
        }

        private void CarregarInternacoes(int idPaciente, int? selectIdInternacao)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"SELECT id_internacao, data_internacao, data_alta
                               FROM Internacoes
                               WHERE id_paciente = @id
                               ORDER BY data_internacao DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                dt.Load(cmd.ExecuteReader());
            }
            gvInternacoes.DataSource = dt;
            gvInternacoes.DataBind();

            // Selecionar a internação nova ou desejada, se aplicável
            if (selectIdInternacao.HasValue)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((int)dt.Rows[i]["id_internacao"] == selectIdInternacao.Value)
                    {
                        gvInternacoes.SelectedIndex = i;
                        CarregarInternacao(selectIdInternacao.Value);
                        return;
                    }
                }
                gvInternacoes.SelectedIndex = -1;
            }
            else
            {
                gvInternacoes.SelectedIndex = -1;
            }
        }

        private void CarregarDadosPaciente(int idPaciente)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = "SELECT nome, nascimento FROM paciente WHERE id_paciente = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idPaciente);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblNome.Text = dr["nome"].ToString();
                    DateTime nascimento = Convert.ToDateTime(dr["nascimento"]);
                    int idade = DateTime.Today.Year - nascimento.Year;
                    if (nascimento > DateTime.Today.AddYears(-idade)) idade--;
                    lblIdade.Text = idade.ToString();
                }
            }
        }

        protected void gvInternacoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparFormulario();
            if (gvInternacoes.SelectedDataKey != null)
            {
                int idInternacao = (int)gvInternacoes.SelectedDataKey.Value;
                CarregarInternacao(idInternacao);
            }
        }

        private void LimparFormulario()
        {
            txtSetor.Text = "";
            txtLeito.Text = "";
            txtDataInternacao.Text = "";
            txtDataAlta.Text = "";
            txtSetor.Enabled = true;
            txtLeito.Enabled = true;
            txtDataInternacao.Enabled = true;
            txtDataAlta.Enabled = true;
            btnSalvar.Enabled = true;
        }

        private void CarregarInternacao(int idInternacao)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"SELECT setor, leito, data_internacao, data_alta FROM Internacoes WHERE id_internacao = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idInternacao);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtSetor.Text = dr["setor"].ToString();
                    txtLeito.Text = dr["leito"].ToString();
                    txtDataInternacao.Text = Convert.ToDateTime(dr["data_internacao"]).ToString("yyyy-MM-dd");
                    if (dr["data_alta"] != DBNull.Value)
                    {
                        txtDataAlta.Text = Convert.ToDateTime(dr["data_alta"]).ToString("yyyy-MM-dd");
                        txtSetor.Enabled = false;
                        txtLeito.Enabled = false;
                        txtDataInternacao.Enabled = false;
                        txtDataAlta.Enabled = false;
                        btnSalvar.Enabled = false;
                    }
                    else
                    {
                        txtDataAlta.Text = "";
                        txtSetor.Enabled = true;
                        txtLeito.Enabled = true;
                        txtDataInternacao.Enabled = true;
                        txtDataAlta.Enabled = true;
                        btnSalvar.Enabled = true;
                    }
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string setor = txtSetor.Text.Trim();
            string leito = txtLeito.Text.Trim();
            if (string.IsNullOrEmpty(setor) || string.IsNullOrEmpty(leito) || string.IsNullOrEmpty(txtDataInternacao.Text.Trim()))
            {
                lblMsg.Text = "Preencha todos os campos obrigatórios.";
                return;
            }
            if (!DateTime.TryParse(txtDataInternacao.Text.Trim(), out DateTime dataInternacao))
            {
                lblMsg.Text = "Data de internação inválida.";
                return;
            }
            DateTime? dataAlta = null;
            if (!string.IsNullOrWhiteSpace(txtDataAlta.Text) && DateTime.TryParse(txtDataAlta.Text.Trim(), out DateTime dtAlta))
                dataAlta = dtAlta;

            // Se está com alguma linha selecionada, atualiza; se não, insere nova e deixa nova selecionada!
            if (gvInternacoes.SelectedDataKey != null)
            {
                int idInternacao = (int)gvInternacoes.SelectedDataKey.Value;
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    string sql = @"UPDATE Internacoes SET setor=@setor, leito=@leito, data_internacao=@dataInternacao, data_alta=@dataAlta WHERE id_internacao=@id";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@setor", setor);
                    cmd.Parameters.AddWithValue("@leito", leito);
                    cmd.Parameters.AddWithValue("@dataInternacao", dataInternacao);
                    cmd.Parameters.AddWithValue("@dataAlta", (object)dataAlta ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", idInternacao);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMsg.Text = "Atualizado com sucesso!";
                CarregarInternacoes(idPaciente, idInternacao);
            }
            else
            {
                int novoId = 0;
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    string sql = @"INSERT INTO Internacoes (id_paciente, setor, leito, data_internacao, data_alta)
                                   OUTPUT INSERTED.id_internacao
                                   VALUES (@idPaciente, @setor, @leito, @dataInternacao, @dataAlta)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@idPaciente", idPaciente);
                    cmd.Parameters.AddWithValue("@setor", setor);
                    cmd.Parameters.AddWithValue("@leito", leito);
                    cmd.Parameters.AddWithValue("@dataInternacao", dataInternacao);
                    cmd.Parameters.AddWithValue("@dataAlta", (object)dataAlta ?? DBNull.Value);
                    con.Open();
                    novoId = (int)cmd.ExecuteScalar();
                }
                lblMsg.Text = "Nova internação cadastrada!";
                CarregarInternacoes(idPaciente, novoId);
                LimparFormulario();
            }
            // Dados do grid são sempre atualizados e selecionado
        }

        protected void btnNovaInternacao_Click(object sender, EventArgs e)
        {
            LimparFormulario();
            gvInternacoes.SelectedIndex = -1; // Também deselect a seleção da lista
            lblMsg.Text = ""; // Limpa mensagens
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace ProjetoIntegrador
{
    public partial class CadastroUsu : System.Web.UI.Page
    {
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string registro = txtRegistro.Text.Trim();
            string telefone = txtTelefone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;
            int idGrupo = int.Parse(ddlGrupo.SelectedValue);
            bool ativo = chkAtivo.Checked;

            // Validação: obrigatório
            if (string.IsNullOrWhiteSpace(nome))
            {
                Alerta("O campo Nome é obrigatório.");
                return;
            }
            if (string.IsNullOrWhiteSpace(registro))
            {
                Alerta("O campo Registro é obrigatório.");
                return;
            }
            if (string.IsNullOrWhiteSpace(telefone))
            {
                Alerta("O campo Telefone é obrigatório.");
                return;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                Alerta("O campo Email é obrigatório.");
                return;
            }
            if (!ValidarEmail(email))
            {
                Alerta("Informe um email válido.");
                return;
            }
            if (string.IsNullOrWhiteSpace(senha) || senha.Length < 6)
            {
                Alerta("A senha deve ter pelo menos 6 caracteres.");
                return;
            }
            if (!Regex.IsMatch(senha, @"[A-Za-z]") || !Regex.IsMatch(senha, @"\d"))
            {
                Alerta("A senha deve conter letras e números.");
                return;
            }

            // Checar já registrados (usuário e email)
            string connStr = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sqlCheckEmail = "SELECT COUNT(1) FROM usuario WHERE LOWER(email) = LOWER(@email)";
                using (SqlCommand cmd = new SqlCommand(sqlCheckEmail, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        Alerta("Já existe um usuário com esse email.");
                        return;
                    }
                }

                string sqlCheckReg = "SELECT COUNT(1) FROM usuario WHERE registro = @registro";
                using (SqlCommand cmd = new SqlCommand(sqlCheckReg, conn))
                {
                    cmd.Parameters.AddWithValue("@registro", registro);
                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        Alerta("Já existe um usuário com esse registro.");
                        return;
                    }
                }

                // Cadastro
                string senhaHash = GerarHashSenha(senha);

                string sql = @"INSERT INTO usuario 
                           (id_gp_usuario, nome, registro, telefone, email, senha_hash, ativo)
                           VALUES (@id_gp_usuario, @nome, @registro, @telefone, @email, @senha_hash, @ativo)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_gp_usuario", idGrupo);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@registro", registro);
                    cmd.Parameters.AddWithValue("@telefone", telefone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@senha_hash", senhaHash);
                    cmd.Parameters.AddWithValue("@ativo", ativo);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        Alerta("Usuário cadastrado com sucesso!");
                        LimparCampos();
                    }
                    catch (Exception ex)
                    {
                        Alerta("Erro ao cadastrar: " + ex.Message);
                    }
                }
            }
        }

        private void Alerta(string mensagem)
        {
            // Beleza para alertas rápidos (substituir por lblMsg.Text se preferir inline na página)
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{mensagem}');", true);
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtRegistro.Text = "";
            txtTelefone.Text = "";
            txtEmail.Text = "";
            txtSenha.Text = "";
            ddlGrupo.SelectedIndex = 0;
            chkAtivo.Checked = true;
        }

        private string GerarHashSenha(string senha)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private bool ValidarEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}

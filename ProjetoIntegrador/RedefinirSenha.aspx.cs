using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjetoIntegrador
{
    public partial class RedefinirSenha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Apenas no primeiro carregamento da página (não no postback)
            if (!IsPostBack)
            {
                // Pega o telefone da URL
                string telefoneUsuario = Request.QueryString["telefone"];
                if (!string.IsNullOrWhiteSpace(telefoneUsuario))
                {
                    // Armazena o telefone no campo oculto para que ele persista no postback
                    hdnTelefone.Value = telefoneUsuario;
                }
                else
                {
                    lblMensagem.Text = "Sessão expirada. Por favor, reinicie o processo de redefinição de senha.";
                }
            }
        }
        protected void btnRedefinir_Click(object sender, EventArgs e)
        {
            string codigoDigitado = txtCodigo.Text.Trim();
            string novaSenha = txtNovaSenha.Text.Trim();

            // Pega o telefone do campo oculto, que persiste após o postback
            string telefoneUsuario = hdnTelefone.Value;

            if (string.IsNullOrWhiteSpace(telefoneUsuario))
            {
                lblMensagem.Text = "Sessão expirada. Por favor, reinicie o processo de redefinição de senha.";
                return;
            }

            if (string.IsNullOrWhiteSpace(codigoDigitado))
            {
                lblMensagem.Text = "Digite o código de verificação.";
                return;
            }

            // Busca o código no banco de dados
            string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Codigo, DataExpiracao FROM CodigoRedefinicao WHERE Telefone = @telefone";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@telefone", telefoneUsuario);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            string codigoGerado = reader["Codigo"].ToString();
                            DateTime dataExpiracao = (DateTime)reader["DataExpiracao"];

                            // Valida se o código não expirou e se é o mesmo digitado pelo usuário
                            if (DateTime.Now > dataExpiracao)
                            {
                                lblMensagem.Text = "O código expirou.";
                                reader.Close();
                                return;
                            }

                            if (!string.Equals(codigoDigitado, codigoGerado, StringComparison.Ordinal))
                            {
                                lblMensagem.Text = "Código inválido.";
                                reader.Close();
                                return;
                            }
                            reader.Close();
                        }
                        else
                        {
                            lblMensagem.Text = "Código inválido ou não encontrado.";
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro ao buscar código: " + ex.Message;
                        return;
                    }
                }
            }

            // Se o código for válido, prossegue com a atualização da senha
            if (string.IsNullOrWhiteSpace(novaSenha))
            {
                lblMensagem.Text = "Digite a nova senha.";
                return;
            }

            // Cria hash SHA256
            string senhaHash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(novaSenha));
                senhaHash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }

            // Atualiza senha no banco
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE usuario SET senha_hash = @senhaHash WHERE telefone = @telefone AND ativo = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@senhaHash", senhaHash);
                    cmd.Parameters.AddWithValue("@telefone", telefoneUsuario);

                    try
                    {
                        conn.Open();
                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            // Remove o código temporário do banco de dados após o sucesso
                            string deleteQuery = "DELETE FROM CodigoRedefinicao WHERE Telefone = @telefone";
                            using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                            {
                                deleteCmd.Parameters.AddWithValue("@telefone", telefoneUsuario);
                                deleteCmd.ExecuteNonQuery();
                            }

                            lblMensagem.Text = "Senha redefinida com sucesso!";
                        }
                        else
                        {
                            lblMensagem.Text = "Usuário não encontrado ou inativo.";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro ao atualizar senha: " + ex.Message;
                    }
                }
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjetoIntegrador
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string senha = txtSenha.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(senha))
            {
                lblMensagem.Text = "Por favor, preencha todos os campos.";
                return;
            }

            // Gera o hash da senha em MAIÚSCULAS
            string senhaHash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                senhaHash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                                SELECT u.id_usuario, u.nome, g.nome, g.id_gp_usuario
                                FROM usuario u
                                INNER JOIN grupo_usuario g ON u.id_gp_usuario = g.id_gp_usuario
                                WHERE u.nome = @nome
                                AND u.senha_hash = @senhaHash
                                AND u.ativo = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@senhaHash", senhaHash);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Armazena informações do usuário na sessão
                                Session["UserID"] = reader["id_usuario"];
                                Session["UserName"] = reader["nome"];
                                Session["UserGroup"] = reader["nome"];
                                Session["UserGroupID"] = reader["id_gp_usuario"];

                                // Verifica se é admin (adapte conforme o nome exato salvo no seu banco)
                                string grupo = reader["nome"].ToString().ToLower();
                                if (grupo.Contains("admin"))
                                {
                                    Response.Redirect("HomeAdmin.aspx");
                                }
                                else
                                {
                                    Response.Redirect("Home.aspx");
                                }
                            }
                            else
                            {
                                lblMensagem.Text = "Usuário ou senha inválidos.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro: " + ex.Message;
                    }
                }
            }
        }

        protected void btnEsqueciSenha_Click(object sender, EventArgs e)
        {
            inputTelefone.Visible = true;
            btncodigo.Visible = true;
            lblMensagem.Text = "Por favor, insira o número de telefone cadastrado em sua conta. Enviaremos um código por SMS.";
            lblMensagem.Visible = true;
        }

        protected async void btncodigo_Click(object sender, EventArgs e)
        {
            string telefone = inputTelefone.Text.Trim();
            Random random = new Random();
            int codigo = random.Next(10000, 100000);

            if (string.IsNullOrWhiteSpace(telefone))
            {
                lblMensagem.Text = "Digite seu telefone para redefinir a senha.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand getIdCmd = new SqlCommand("SELECT id_usuario FROM usuario WHERE telefone = @telefone", conn);
                getIdCmd.Parameters.AddWithValue("@telefone", telefone);
                object result = getIdCmd.ExecuteScalar();

                if (result == null)
                {
                    lblMensagem.Text = "Telefone não encontrado.";
                    return;
                }
            }

            // DELETADO: Não estamos mais salvando o telefone na sessão
            // Salva o telefone do usuário na sessão para que a página de redefinição possa buscá-lo
            // Session["TelefoneUsuario"] = telefone;

            // Salva o código temporário no banco de dados
            string connectionStringCodigo = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection connCodigo = new SqlConnection(connectionStringCodigo))
            {
                // Deleta códigos antigos para o mesmo telefone
                string deleteQuery = "DELETE FROM CodigoRedefinicao WHERE Telefone = @telefone";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connCodigo))
                {
                    deleteCmd.Parameters.AddWithValue("@telefone", telefone);
                    connCodigo.Open();
                    deleteCmd.ExecuteNonQuery();
                }

                // Insere o novo código
                string insertQuery = "INSERT INTO CodigoRedefinicao (Telefone, Codigo, DataExpiracao) VALUES (@telefone, @codigo, @dataExpiracao)";
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, connCodigo))
                {
                    insertCmd.Parameters.AddWithValue("@telefone", telefone);
                    insertCmd.Parameters.AddWithValue("@codigo", codigo.ToString());
                    insertCmd.Parameters.AddWithValue("@dataExpiracao", DateTime.Now.AddMinutes(15)); // Expira em 15 minutos

                    if (connCodigo.State != System.Data.ConnectionState.Open)
                    {
                        connCodigo.Open();
                    }
                    insertCmd.ExecuteNonQuery();
                }
            }

            // Dados do Twilio
            string accountSid = "ACa6e52d9053bde89a6ec42c1b02db2415";
            string authToken = "16ce6b1cc342a27d46d53198a7290d2e";
            string fromNumber = "+12569253443"; // Número de teste do Twilio
            string toNumber = "+55" + telefone; // Número do destinatário
            string message = "Seu código é: " + codigo;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/");
                    var byteArray = Encoding.ASCII.GetBytes($"{accountSid}:{authToken}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("To", toNumber),
                        new KeyValuePair<string, string>("From", fromNumber),
                        new KeyValuePair<string, string>("Body", message)
                    });
                    await client.PostAsync("Messages.json", content);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao enviar o código: " + ex.Message;
                // Continua o fluxo mesmo que o SMS falhe, pois o código está no banco de dados.
            }

            lblMensagem.Text = "Código enviado. Redirecionando...";
            // CORREÇÃO: Passa o telefone na URL para que a próxima página possa buscá-lo
            Response.Redirect("RedefinirSenha.aspx?telefone=" + telefone);
        }
    }
}


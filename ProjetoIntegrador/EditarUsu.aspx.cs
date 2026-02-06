using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjetoIntegrador
{
	public partial class EditarUsu : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarGrupos(); // Carrega opções do dropdown dos grupos
                if (Request.QueryString["id"] != null)
                {
                    int idUsuario = int.Parse(Request.QueryString["id"]);
                    CarregarUsuario(idUsuario);
                }
            }
        }

        private void CarregarGrupos()
        {
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = "SELECT id_gp_usuario, nome FROM grupo_usuario";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ddlGrupo.DataSource = dr;
                ddlGrupo.DataTextField = "nome";
                ddlGrupo.DataValueField = "id_gp_usuario";
                ddlGrupo.DataBind();
            }
        }

        private void CarregarUsuario(int idUsuario)
        {
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"SELECT nome, registro, telefone, email, id_gp_usuario, ativo
                               FROM usuario WHERE id_usuario = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtNome.Text = dr["nome"].ToString();
                    txtRegistro.Text = dr["registro"].ToString();
                    txtTelefone.Text = dr["telefone"].ToString();
                    txtEmail.Text = dr["email"].ToString();
                    ddlGrupo.SelectedValue = dr["id_gp_usuario"].ToString();
                    chkAtivo.Checked = dr["ativo"].ToString() == "1";
                }
            }
        }

        protected void SalvarUsu_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
            {
                // Caso de uso seguro, nunca ocorre vindo da edição via grid
                lblMensagem.Text = "Usuário não identificado.";
                return;
            }

            int idUsuario = int.Parse(Request.QueryString["id"]);
            string nome = txtNome.Text.Trim();
            string registro = txtRegistro.Text.Trim();
            string telefone = txtTelefone.Text.Trim();
            string email = txtEmail.Text.Trim();
            int idGrupo = int.Parse(ddlGrupo.SelectedValue);
            bool ativo = chkAtivo.Checked;

            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"UPDATE usuario
                        SET nome = @nome,
                            registro = @registro,
                            telefone = @telefone,
                            email = @email,
                            id_gp_usuario = @idGrupo,
                            ativo = @ativo
                        WHERE id_usuario = @idUsuario";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@registro", registro);
                cmd.Parameters.AddWithValue("@telefone", telefone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@idGrupo", idGrupo);
                cmd.Parameters.AddWithValue("@ativo", ativo ? 1 : 0);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Opcional: Mensagem de feedback (adicione um Label lblMensagem no aspx se quiser)
            lblMensagem.Text = "Usuário atualizado com sucesso!";

            // Redireciona para a tela de admin após salvar
            Response.Redirect("HomeAdmin.aspx");
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
            {
                // Segurança, geralmente não ocorre neste cenário
                lblMensagem.Text = "Usuário não identificado.";
                return;
            }

            int idUsuario = int.Parse(Request.QueryString["id"]);
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = "DELETE FROM usuario WHERE id_usuario = @idUsuario";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Mensagem opcional (adicione <asp:Label ID="lblMensagem" ...> ao seu aspx se for usar)
            lblMensagem.Text = "Usuário excluído com sucesso!";

            // Redireciona para a tela de admin após excluir
            Response.Redirect("HomeAdmin.aspx");
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjetoIntegrador
{
	public partial class HomeAdmin : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                CarregarUsuarios();
            }
        }

        private void CarregarUsuarios()
        {
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = @"
                    SELECT u.id_usuario, u.nome, u.registro, u.telefone, u.email, g.nome AS nome_grupo, 
                           CASE WHEN u.ativo = 1 THEN 'Sim' ELSE 'Não' END AS ativo
                    FROM usuario u
                    INNER JOIN grupo_usuario g ON u.id_gp_usuario = g.id_gp_usuario
                ";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvUsuarios.DataSource = dt;
                gvUsuarios.DataBind();
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("EditarUsu.aspx?id=" + idUsuario);
            }
            else if (e.CommandName == "Excluir")
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                // Aqui você pode implementar a lógica de exclusão, se desejar.
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
			Response.Redirect("Login.aspx");
        }

        protected void btnCadastroUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("CadastroUsu.aspx");
        }
    }
}
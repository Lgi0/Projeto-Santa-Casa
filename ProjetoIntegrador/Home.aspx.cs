using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjetoIntegrador
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] != null)
                    lblOlaUsuario.Text = "Olá, " + Session["UserName"].ToString();
                else
                    lblOlaUsuario.Text = "Olá, visitante";
                CarregarPacientes(""); // só NA PRIMEIRA VEZ
            }
        }

        private void CarregarPacientes(string filtro)
        {
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = "SELECT id_paciente, nome, nascimento, telefone, email, endereco, cpf FROM paciente";
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    sql += " WHERE nome LIKE @filtro OR cpf LIKE @filtrocpf";
                }

                SqlCommand cmd = new SqlCommand(sql, con);
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    cmd.Parameters.AddWithValue("@filtrocpf", "%" + filtro.Replace(".", "").Replace("-", "") + "%");
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                gvPacientes.DataSource = dt;
                gvPacientes.DataBind();
            }
        }
       


        protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPacientes.PageIndex = e.NewPageIndex;
            string busca = txtPesquisa.Text.Trim();
            CarregarPacientes(busca);
        }


        protected void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            string busca = txtPesquisa.Text.Trim();
            gvPacientes.PageIndex = 0; // Sempre volta para a primeira página ao pesquisar!
            CarregarPacientes(busca);
        }


        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Editar":
                    {
                        int idPaciente = Convert.ToInt32(e.CommandArgument);
                        Session["ID_PACIENTE"] = idPaciente;
                        Response.Redirect("EditarPac.aspx?id=" + idPaciente);
                    }
                    break;

                case "PTA":
                    {
                        int idPaciente = Convert.ToInt32(e.CommandArgument);
                        Session["ID_PACIENTE"] = idPaciente;
                        Response.Redirect("PTA.aspx?id=" + idPaciente);
                    }
                    break;

                case "PTS":
                    Response.Redirect("PTS.aspx");
                    break;
            }
        }



        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        protected void btnCadastroPac_Click(object sender, EventArgs e)
        {
            Response.Redirect("CadastroPac.aspx");
        }

        protected void btnSair_Click1(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}
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
	public partial class ConsultarPaciente : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarPacientes();
            }
        }

        private void CarregarPacientes()
        {
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = "SELECT id_paciente, nome, nascimento, telefone, email, endereco, esf FROM paciente";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPacientes.DataSource = dt;
                gvPacientes.DataBind();
            }
        }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idPaciente = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Editar":
                    Response.Redirect("EditarPac.aspx?id=" + idPaciente);
                    break;

                case "PTS":
                    Response.Redirect("PTS.aspx?id=" + idPaciente);
                    break;

                case "PTA":
                    Response.Redirect("PTA.aspx?id=" + idPaciente);
                    break;
            }
        }

    }
}
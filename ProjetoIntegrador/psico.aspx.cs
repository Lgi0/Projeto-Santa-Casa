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
    public partial class psico : System.Web.UI.Page
    {
        string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (Session["ID_PACIENTE"] != null)
                {
                    int idPaciente = (int)Session["ID_PACIENTE"];
                    CarregarPaciente(idPaciente);
                }
                else
                {
                    Response.Redirect("Home.aspx");
                }
            }
        }

        private void CarregarPaciente(int id)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string sql = "SELECT nome, nascimento, cpf, telefone, email, endereco, esf FROM paciente WHERE id_paciente=@id";
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

        protected void btnEnfermagem_Click(object sender, EventArgs e)
        {
            Response.Redirect("pta_enfermagem.aspx");
        }

        protected void btnFisioterapia_Click(object sender, EventArgs e)
        {
            Response.Redirect("pta_fisioterapia.aspx");
        }

        protected void btnNutricao_Click(object sender, EventArgs e)
        {
            Response.Redirect("pta_nutricao.aspx");
        }

        protected void btnPsicologia_Click(object sender, EventArgs e)
        {
            Response.Redirect("pta_psicologia.aspx");
        }

        protected void btnSocial_Click(object sender, EventArgs e)
        {
            Response.Redirect("pta_social.aspx");
        }


        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session.Remove("ID_PACIENTE");
            Response.Redirect("Home.aspx");
        }
    }
}
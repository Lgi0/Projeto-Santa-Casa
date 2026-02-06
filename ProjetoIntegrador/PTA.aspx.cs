using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjetoIntegrador
{
    public partial class PTA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idPaciente = Convert.ToInt32(Request.QueryString["id"]);

            Session["ID_PACIENTE"] = idPaciente;
        }

        protected void btnEnfermagem_Click(object sender, EventArgs e)
        {
            var perfil = Session["UserGroup"] as string;
            if (perfil != null && perfil.ToLower().Contains("enferm"))
            {
                // Recupere o id do paciente da QueryString OU da Session
                int idPaciente = -1;
                if (Request.QueryString["id"] != null)
                {
                    idPaciente = Convert.ToInt32(Request.QueryString["id"]);
                }
                else if (Session["ID_PACIENTE"] != null)
                {
                    idPaciente = Convert.ToInt32(Session["ID_PACIENTE"]);
                }
                else
                {
                    Response.Redirect("Home.aspx");
                    return;
                }

                // **Redirecione levando o ID**
                Response.Redirect("pta_enfermagem.aspx?id=" + idPaciente);
            }
            else
            {
                Response.Redirect("AcessoNegado.aspx");
            }
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
            if(perfil != null && perfil.ToLower().Contains("psicó"))
                Response.Redirect("pta_psicologia.aspx");
            else
                Response.Redirect("pta_psicologia.aspx");
        }

        protected void btnSocial_Click(object sender, EventArgs e)
        {
            var perfil = Session["UserGroup"] as string;
            if (perfil != null && perfil.ToLower().Contains("social"))
                Response.Redirect("pta_social.aspx");
            else
                Response.Redirect("AcessoNegado.aspx");
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}
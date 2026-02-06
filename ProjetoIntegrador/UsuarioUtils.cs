using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoIntegrador
{
	public class UsuarioUtils
	{
        public static string GetUserGroup()
        {
            if (HttpContext.Current.Session["UserGroup"] != null)
                return HttpContext.Current.Session["UserGroup"].ToString();
            return "";
        }

        public static int GetUserGroupID()
        {
            if (HttpContext.Current.Session["UserGroupID"] != null)
                return Convert.ToInt32(HttpContext.Current.Session["UserGroupID"]);
            return 0;
        }

        public static bool IsEnfermeiro()
        {
            return GetUserGroup().ToLower().Contains("enferm");
        }

        public static bool IsNutricionista()
        {
            return GetUserGroup().ToLower().Contains("nutri");
        }

        public static bool IsFisioterapeuta()
        {
            return GetUserGroup().ToLower().Contains("fisio");
        }

        public static bool IsPsicologo()
        {
            return GetUserGroup().ToLower().Contains("psico");
        }

        public static bool IsServicoSocial()
        {
            return GetUserGroup().ToLower().Contains("social");
        }

        public static bool IsAdmin()
        {
            return GetUserGroup().ToLower().Contains("admin");
        }

        public static void VerificarLogin()
        {
            if (HttpContext.Current.Session["UserID"] == null)
            {
                HttpContext.Current.Response.Redirect("Login.aspx");
            }
        }
    }
}
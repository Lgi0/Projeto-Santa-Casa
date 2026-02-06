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
    public partial class EditarPac : System.Web.UI.Page
    {

        string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int idPaciente = Convert.ToInt32(Request.QueryString["id"]);

                    Session["ID_PACIENTE"] = idPaciente;

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

                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtNome.Text = dr["nome"].ToString();
                        txtCPF.Text = dr["cpf"].ToString();
                        txtTelefone.Text = dr["telefone"].ToString();
                        txtEmail.Text = dr["email"].ToString();
                        txtEndereco.Text = dr["endereco"].ToString();
                        txtEsf.Text = dr["esf"].ToString();

                        if (dr["nascimento"] != DBNull.Value)
                        {
                            DateTime dataNascimento = Convert.ToDateTime(dr["nascimento"]);
                            txtNascimento.Text = dataNascimento.ToString("yyyy-MM-dd");
                        }

                    }
                    else
                    {
                        Response.Write("<script>alert('Paciente não encontrado.');</script>");
                        Response.Redirect("Home.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Erro ao carregar paciente: {ex.Message}');</script>");
                }
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Session.Remove("ID_PACIENTE");
            Response.Redirect("Home.aspx");
        }

        protected void SalvarPac_Click(object sender, EventArgs e)
        {
            if (Session["ID_PACIENTE"] == null)
            {
                Response.Write("<script>alert('Erro: ID do paciente não encontrado na sessão. Retornando à Home.');</script>");
                Response.Redirect("Home.aspx");
                return;
            }

            int idPaciente = (int)Session["ID_PACIENTE"];

            try
            {
                string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

                using (SqlConnection con = new SqlConnection(strCon))
                {
                    string sql = @"UPDATE paciente SET
                               nome = @nome,
                               nascimento = @nascimento,
                               cpf = @cpf,
                               telefone = @telefone,
                               email = @email,
                               endereco = @endereco,
                               esf = @esf
                               WHERE id_paciente = @id_paciente";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_paciente", idPaciente);
                        cmd.Parameters.AddWithValue("@nome", txtNome.Text);

                        DateTime dataNascimento;
                        if (DateTime.TryParse(txtNascimento.Text, out dataNascimento))
                        {
                            cmd.Parameters.AddWithValue("@nascimento", dataNascimento);
                        }
                        else
                        {
                            throw new Exception("Formato de data de nascimento inválido.");
                        }

                        cmd.Parameters.AddWithValue("@cpf", txtCPF.Text);
                        cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                        cmd.Parameters.AddWithValue("@esf", txtEsf.Text);

                        con.Open();
                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            Response.Redirect("Home.aspx");
                        }
                        else
                        {
                            Response.Write("<script>alert('Atenção: Nenhum registro de paciente foi atualizado. Verifique se o ID existe.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Erro ao atualizar paciente: " + ex.Message + "');</script>");
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            // 1. Verifica se o ID do paciente está disponível na Session
            if (Session["ID_PACIENTE"] == null)
            {
                Response.Write("<script>alert('Erro: ID do paciente não encontrado na sessão. Não foi possível excluir.');</script>");
                return;
            }

            int idPaciente = (int)Session["ID_PACIENTE"];
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    // Comando SQL para DELETE
                    string sql = "DELETE FROM paciente WHERE id_paciente = @id_paciente";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_paciente", idPaciente);

                        con.Open();
                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            // 2. Exclusão bem-sucedida: Limpa a Session e volta para Home
                            Session.Remove("ID_PACIENTE");
                            Response.Write("<script>alert('Paciente excluído com sucesso.');</script>");
                            Response.Redirect("Home.aspx");
                        }
                        else
                        {
                            Response.Write("<script>alert('Atenção: Nenhum registro foi excluído. O paciente pode já ter sido removido.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Se houver erro de chave estrangeira (FK), avisa o usuário.
                if (ex.Message.Contains("FK_"))
                {
                    Response.Write("<script>alert('Erro: Não é possível excluir o paciente pois há registros associados a ele em outras tabelas (Ex: prontuário, consultas).');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Erro ao excluir paciente: " + ex.Message + "');</script>");
                }
            }
        }
    }
}
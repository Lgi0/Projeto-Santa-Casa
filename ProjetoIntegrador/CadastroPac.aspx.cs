using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace ProjetoIntegrador
{
    public partial class CadastrarPaciente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void CadastrarPaciente_Click(object sender, EventArgs e)
        {
            // Campos
            string nome = txtNome.Text.Trim();
            string cpf = txtCPF.Text.Trim();
            string telefone = txtTelefone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string endereco = txtEndereco.Text.Trim();
            string esf = txtEsf.Text.Trim();
            string nascimentoStr = txtNascimento.Text.Trim();

            // Validações obrigatórias
            if (string.IsNullOrWhiteSpace(nome))
            {
                Alerta("Nome é obrigatório.");
                return;
            }
            if (string.IsNullOrWhiteSpace(nascimentoStr))
            {
                Alerta("Data de nascimento é obrigatória.");
                return;
            }
            DateTime nascimento;
            if (!DateTime.TryParse(nascimentoStr, out nascimento))
            {
                Alerta("Formato de data de nascimento inválido.");
                return;
            }
            if (nascimento > DateTime.Now)
            {
                Alerta("Data de nascimento não pode ser no futuro.");
                return;
            }
            if (string.IsNullOrWhiteSpace(cpf))
            {
                Alerta("CPF é obrigatório.");
                return;
            }
            if (!ValidarCpf(cpf))
            {
                Alerta("CPF inválido ou em formato incorreto.");
                return;
            }
            if (string.IsNullOrWhiteSpace(telefone) || telefone.Length < 8)
            {
                Alerta("Telefone é obrigatório e deve ter ao menos 8 dígitos.");
                return;
            }
            if (string.IsNullOrWhiteSpace(endereco))
            {
                Alerta("Endereço é obrigatório.");
                return;
            }
            if (string.IsNullOrWhiteSpace(esf))
            {
                Alerta("ESF é obrigatório.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(email) && !ValidarEmail(email))
            {
                Alerta("O email está em formato inválido.");
                return;
            }

            // Checar CPF já cadastrado
            string cpfLimpo = new Regex(@"[^\d]").Replace(cpf, "");
            string strCon = ConfigurationManager.ConnectionStrings["MinhaConexao"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();
                SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(1) FROM paciente WHERE cpf = @cpf", con);
                cmdCheck.Parameters.AddWithValue("@cpf", cpfLimpo);
                if ((int)cmdCheck.ExecuteScalar() > 0)
                {
                    Alerta("Já existe um paciente com este CPF.");
                    return;
                }
            }

            try
            {
                using (SqlConnection con = new SqlConnection(strCon))
                {
                    string sql = @"INSERT INTO paciente 
                        (nome, nascimento, cpf, telefone, email, endereco, esf) 
                        VALUES (@nome, @nascimento, @cpf, @telefone, @email, @endereco, @esf)";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@nascimento", nascimento);
                        cmd.Parameters.AddWithValue("@cpf", cpfLimpo);
                        cmd.Parameters.AddWithValue("@telefone", telefone);
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email);
                        cmd.Parameters.AddWithValue("@endereco", endereco);
                        cmd.Parameters.AddWithValue("@esf", esf);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Redirect("Home.aspx");
            }
            catch (Exception ex)
            {
                Alerta("Erro: " + ex.Message);
            }
        }

        private void Alerta(string mensagem)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{mensagem}');", true);
        }

        private bool ValidarCpf(string cpf)
        {
            cpf = new Regex(@"[^\d]").Replace(cpf, "");
            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
                return false;
            switch (cpf)
            {
                case "00000000000":
                case "11111111111":
                case "22222222222":
                case "33333333333":
                case "44444444444":
                case "55555555555":
                case "66666666666":
                case "77777777777":
                case "88888888888":
                case "99999999999":
                    return false;
            }
            int[] mult1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * mult1[i];
            int resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;
            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * mult2[i];
            resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;
            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        private bool ValidarEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}

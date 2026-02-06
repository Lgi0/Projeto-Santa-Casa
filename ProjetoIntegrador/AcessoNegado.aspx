<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcessoNegado.aspx.cs" Inherits="ProjetoIntegrador.AcessoNegado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Acesso Negado</title>
    <style>
        .erro-container {
            max-width: 400px;
            margin: 80px auto;
            padding: 30px;
            background: #fff;
            border-radius: 8px;
            border: 1.5px solid #e74c3c;
            box-shadow: 0 2px 12px rgba(0,0,0,0.10);
            text-align: center;
        }
        .erro-titulo {
            color: #e74c3c;
            font-size: 2em;
            margin-bottom: 10px;
            font-weight: bold;
        }
        .erro-msg {
            color: #333;
            font-size: 1.15em;
            margin-bottom: 20px;
        }
        .home-btn {
            display: inline-block;
            background: #3498db;
            color: #fff;
            padding: 10px 28px;
            border-radius: 5px;
            text-decoration: none;
            font-size: 1em;
            transition: background 0.2s;
        }
        .home-btn:hover {
            background: #217dbb;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="erro-container">
            <div class="erro-titulo">Acesso Negado</div>
            <div class="erro-msg">Você não tem permissão para acessar esta página.</div>
            <a href="Home.aspx" class="home-btn">Voltar para a Home</a>
        </div>
    </form>
</body>
</html>
2. No code-behind (AcessoNegado.aspx.cs) mantenha padrão:
csharp
namespace SeuProjeto
{
    public partial class AcessoNegado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Opcional: Você pode registrar logs ou enviar alertas aqui
        }
    }
}
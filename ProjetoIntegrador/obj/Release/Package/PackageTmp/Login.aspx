<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjetoIntegrador.WebForm2" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/> <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-LN+7fdVzj6u52u30Kp6M/trliBMCMKTyK833zpbD+pXdCLuTusPj697FH4R/5mcr" crossorigin="anonymous">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js" integrity="sha384-ndDqU0Gzau9qJ1lfW4pNLlhNTkCfHzAVBReH9diLvGRem5+R9g2FzA8ZGN954O5Q" crossorigin="anonymous"></script>
    <title>Login</title>
</head>
<body>
    
     <form id="form1" runat="server">
        <div class="card" style="width: 30rem;">
            <img src="https://examinadiagnosticos.com.br/wp-content/uploads/2021/03/santa-casa-de-adamantina.png" class="card-img-top" alt="logo-santa-casa">
            <div class="card-body">
                <div class="form-group">
                    <label for="txtNome">Login</label>
                    <asp:TextBox ID="txtNome" CssClass="form-control" runat="server" />
                </div>
                <div class="form-group">
                    <label for="txtSenha">Senha</label>
                    <asp:TextBox ID="txtSenha" TextMode="Password" CssClass="form-control" runat="server" />
                </div>
                <br />
                <asp:Button ID="btnEntrar" CssClass="btn btn-primary" runat="server" Text="Entrar" OnClick="btnEntrar_Click" />
                <asp:Button ID="btnEsqueciSenha" CssClass="btn btn-link" runat="server" Text="Esqueci minha senha?" OnClick="btnEsqueciSenha_Click"/>
                <br /><br />
                <asp:Label ID="lblMensagem" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox Visible="false" TextMode="SingleLine" ID="inputTelefone" runat="server" />
                <asp:Button Visible="false" CssClass="btn btn-primary" ID="btncodigo" Text="Enviar código" OnClick="btncodigo_Click" runat="server" />
            </div>
        </div>
    </form>

    <style>
        .card {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            border-color: deepskyblue;
            border-style: ridge;
            padding: 5px;
            border-width: medium;
            border-radius: 20px;
        }
    </style>
</body>
</html>

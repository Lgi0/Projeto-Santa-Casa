<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RedefinirSenha.aspx.cs" Inherits="ProjetoIntegrador.RedefinirSenha" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Redefinir Senha</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card" style="width: 30rem;">
            <img src="https://examinadiagnosticos.com.br/wp-content/uploads/2021/03/santa-casa-de-adamantina.png" class="card-img-top" alt="logo-santa-casa">
            <div class="card-body">
                <h2 class="text-center mb-3">Redefinir Senha</h2>
                <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" CssClass="mb-2 d-block" />
                <div class="form-group mb-3">
                    <label for="txtCodigo">Código:</label>
                    <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" placeholder="Digite o código recebido" />
                </div>
                <div class="form-group mb-3">
                    <label for="txtNovaSenha">Nova senha:</label>
                    <asp:TextBox ID="txtNovaSenha" runat="server" TextMode="Password" CssClass="form-control" placeholder="Nova senha" />
                </div>
                <asp:HiddenField ID="hdnTelefone" runat="server" />
                <div class="d-grid gap-2 mb-2">
                    <asp:Button ID="btnRedefinir" runat="server" Text="Redefinir" CssClass="btn btn-primary btn-block" OnClick="btnRedefinir_Click" />
                </div>
                <div class="d-grid gap-2 mb-2">
                    <asp:Button ID="btnSair" CssClass="btn btn-outline-danger" Text="Sair" OnClick="btnSair_Click" runat="server" />
                </div>
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

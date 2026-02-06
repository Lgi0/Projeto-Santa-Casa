<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroUsu.aspx.cs" Inherits="ProjetoIntegrador.CadastroUsu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <title>Cadastro</title>
</head>
<body>
    <style>
        .login-box {
            width: 400px;
            margin: 80px auto;
            border: 2px solid #00a1e0;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0px 0px 10px rgba(0,0,0,0.1);
            background-color: #fff;
        }
        .login-box h2 {
            text-align: center;
            margin-bottom: 20px;
            color: #00a1e0;
        }
        .form-control, .form-check-input {
            margin-bottom: 15px;
        }
    </style>

        
             <form id="form1" runat="server">
        <div class="login-box">
            <h2>Cadastro de Usuário</h2>

            <div class="form-group">
                <label for="txtNome">Nome:</label>
                <asp:TextBox ID="txtNome" CssClass="form-control" runat="server" />
            </div>

            <div class="form-group">
                <label for="txtRegistro">Registro:</label>
                <asp:TextBox ID="txtRegistro" CssClass="form-control" runat="server" />
            </div>

            <div class="form-group">
                <label for="txtTelefone">Telefone:</label>
                <asp:TextBox ID="txtTelefone" CssClass="form-control" runat="server" />
            </div>

            <div class="form-group">
                <label for="txtEmail">Email:</label>
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" />
            </div>

            <div class="form-group">
                <label for="txtSenha">Senha:</label>
                <asp:TextBox ID="txtSenha" CssClass="form-control" TextMode="Password" runat="server" />
            </div>

            <div class="form-group">
                <label for="ddlGrupo">Grupo:</label>
                <asp:DropDownList ID="ddlGrupo" CssClass="form-control" runat="server">
                    <asp:ListItem Value="1">Médico</asp:ListItem>
                    <asp:ListItem Value="2">Enfermeiro</asp:ListItem>
                    <asp:ListItem Value="3">Admin</asp:ListItem>
                    <asp:ListItem Value="3">Fisioterapeuta</asp:ListItem>
                    <asp:ListItem Value="4">Nutricionista</asp:ListItem>
                    <asp:ListItem Value="5">Psicólogo</asp:ListItem>
                    <asp:ListItem Value="6">Serviço Social</asp:ListItem>
                    <asp:ListItem Value="7">Fisioterapeuta</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-check">
                <asp:CheckBox ID="chkAtivo" CssClass="form-check-input" runat="server" Checked="true" />
                <label class="form-check-label" for="chkAtivo">Ativo</label>
            </div>

            <div class="text-center mt-3">
                <asp:Button ID="btnSalvar" runat="server" CssClass="btn btn-primary w-100" 
                    Text="Salvar" OnClick="btnSalvar_Click" />
            </div>
        </div>
    </form>

</body>
</html>

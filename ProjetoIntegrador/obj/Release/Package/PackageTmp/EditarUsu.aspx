<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditarUsu.aspx.cs" Inherits="ProjetoIntegrador.EditarUsu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js"></script>
    <title>Editar Usuário</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card">
          <div class="card-body">
              <h1>Edição de Usuário</h1>
              <br />

              <div class="input-group mb-3">
                  <span class="input-group-text">Nome Completo</span>
                  <asp:TextBox ID="txtNome" CssClass="form-control" runat="server"></asp:TextBox>
              </div>

              <div class="input-group mb-3">
                  <span class="input-group-text">Registro</span>
                  <asp:TextBox ID="txtRegistro" CssClass="form-control" runat="server"></asp:TextBox>
              </div>

              <div class="input-group mb-3">
                  <span class="input-group-text">Telefone</span>
                  <asp:TextBox ID="txtTelefone" CssClass="form-control" runat="server"></asp:TextBox>
              </div>

              <div class="input-group mb-3">
                  <span class="input-group-text">E-mail</span>
                  <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
              </div>

              <div class="input-group mb-3">
                  <span class="input-group-text">Grupo</span>
                  <asp:DropDownList ID="ddlGrupo" CssClass="form-select" runat="server"></asp:DropDownList>
              </div>

              <div class="input-group mb-3">
                  <span class="input-group-text">Ativo?</span>
                  <asp:CheckBox ID="chkAtivo" CssClass="form-check-input ms-2" runat="server" />
              </div>

              <asp:Button class="btn btn-primary w-100" Text="Salvar" OnClick="SalvarUsu_Click" ID="SalvarUsu" runat="server" />
              <br /><br />
              
              <asp:Button ID="btnExcluir" runat="server" Text="Excluir"
                  OnClick="btnExcluir_Click"
                  OnClientClick="return confirm('Tem certeza que deseja EXCLUIR este usuário? Esta ação não pode ser desfeita.');"
                  CssClass="btn btn-danger w-100" />
              <br /><br />
              
              <asp:Button ID="btnVoltar" CssClass="btn btn-outline-danger w-100" Text="Voltar" runat="server" OnClick="btnVoltar_Click"/>
              <asp:Label ID="lblMensagem" Text="" runat="server" />
          </div>
        </div>

        <style>
	        .card {
		        border-radius:20px;
		        border-color:lightblue;
		        border-width:2px;
		        position: absolute;
		        top: 50%;
		        left: 50%;
		        transform: translate(-50%, -50%);
		        width: 900px;
		        height: fit-content;
		        padding: 20px;
		        text-align: center;
	        }
	        .input-group-text {
		        background-color:lightblue;
	        }
        </style>
    </form>
</body>
</html>
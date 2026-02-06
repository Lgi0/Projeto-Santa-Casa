<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditarPac.aspx.cs" Inherits="ProjetoIntegrador.EditarPac" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-LN+7fdVzj6u52u30Kp6M/trliBMCMKTyK833zpbD+pXdCLuTusPj697FH4R/5mcr" crossorigin="anonymous">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js" integrity="sha384-ndDqU0Gzau9qJ1lfW4pNLlhNTkCfHzAVBReH9diLvGRem5+R9g2FzA8ZGN954O5Q" crossorigin="anonymous"></script>

    <title>Edição</title>
</head>
<body>
    <form id="form1" runat="server">
                <nav class="navbar navbar-expand-lg fixed-top" style="box-shadow:0 2px 8px rgba(0,0,0,0.02); min-height: 98px; background: rgb(172, 215, 230)">
                    <div class="container-fluid">
                        <div class="row w-100 align-items-center" style="flex-wrap:nowrap;">
                            <!-- Esquerda: Nome/Usuário -->
                            <div class="col-4 d-flex align-items-center">
                                <span class="navbar-brand mb-0 h5" style="margin-left:2px;">
                                    <asp:Label runat="server" ID="lblOlaUsuario" class="h3" />
                                </span>
                            </div>
                            <!-- Centro: Logo -->
                            <div class="col-4 d-flex justify-content-center">
                                <img src="https://examinadiagnosticos.com.br/wp-content/uploads/2021/03/santa-casa-de-adamantina.png"
                                     alt="Logo Santa Casa"
                                     class="img-fluid"
                                     style="max-height: 200px;" />
                            </div>
           
                        </div>
                    </div>
                </nav>
        
        <div class="card">
  <div class="card-body">
      <h1>Edição de Pacientes</h1>
      <br />

      <div class="input-group mb-3">
          <span class="input-group-text">Nome Completo</span>
          <asp:TextBox ID="txtNome" CssClass="form-control" runat="server"></asp:TextBox>
      </div>

      <div class="input-group mb-3">
            <span class="input-group-text">CPF</span>
            <asp:TextBox ID="txtCPF" CssClass="form-control" runat="server"></asp:TextBox>
        </div>

      <div class="input-group mb-3">
          <span class="input-group-text">Data de Nascimento</span>
          <asp:TextBox ID="txtNascimento" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
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
          <span class="input-group-text">Endereço</span>
          <asp:TextBox ID="txtEndereco" CssClass="form-control" runat="server"></asp:TextBox>
      </div>

        <div class="input-group mb-3">
            <span class="input-group-text">ESF</span>
            <asp:TextBox ID="txtEsf" CssClass="form-control" runat="server"></asp:TextBox>
        </div>

      <asp:Button class="btn btn-primary w-100" Text="Cadastrar" OnClick="SalvarPac_Click" ID="SalvarPac" runat="server" />
      <br />
      <br />
        <asp:Button ID="btnExcluir" runat="server" Text="Excluir" 
            OnClick="btnExcluir_Click" 
            OnClientClick="return confirm('Tem certeza que deseja EXCLUIR este paciente? Esta ação não pode ser desfeita.');" 
            CssClass="btn btn-danger w-100" />
        <br />
        <br />
      <asp:Button ID="btnVoltar" CssClass="btn btn-outline-danger w-100" Text="Voltar" runat="server" OnClick="btnVoltar_Click"/>

  </div>
</div>

<style>
	.card {
		border-radius:20px;
		border-color:lightblue;
		border-width:2px;
		position: absolute;
		top: 55%;
		left: 50%;
		transform: translate(-50%, -50%);
		width: 900px;
		height:fit-content;
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

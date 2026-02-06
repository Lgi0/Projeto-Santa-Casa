<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CadastroPac.aspx.cs" Inherits="ProjetoIntegrador.CadastrarPaciente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br /><br /><br />
	
	<div class="card">
	  <div class="card-body">
          <h1>Cadastro de Pacientes</h1>
          <br />

          <div class="input-group mb-3">
              <span class="input-group-text">Nome Completo</span>
              <asp:TextBox ID="txtNome" CssClass="form-control" runat="server"></asp:TextBox>
          </div>

          <div class="input-group mb-3">
              <span class="input-group-text">Data de Nascimento</span>
              <asp:TextBox ID="txtNascimento" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
          </div>

          <div class="input-group mb-3">
                <span class="input-group-text">CPF</span>
                <asp:TextBox ID="txtCPF" MaxLength="11" CssClass="form-control" runat="server"></asp:TextBox>
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


          <asp:Button class="btn btn-primary w-100" Text="Cadastrar" ID="CadastrarPaciente" OnClick="CadastrarPaciente_Click" runat="server" />
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
			height:fit-content;
			padding: 20px;
			text-align: center;
		}
		.input-group-text {
			background-color:lightblue;
		}
    </style>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeAdmin.aspx.cs" Inherits="ProjetoIntegrador.HomeAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Painel de Administração de Usuários</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg bg-body-tertiary fixed-top" style="box-shadow:0 2px 8px rgba(0,0,0,0.02);">
            <div class="container-fluid">
                <span class="navbar-brand mb-0 h5" style="margin-left:2px;">
                    <asp:Label runat="server" ID="lblOlaUsuario" CssClass="ola-usuario"/>
                </span>
                <div class="d-flex ms-auto">
                    <asp:Button ID="btnSair" CssClass="btn btn-outline-danger" Text="Sair" OnClick="btnSair_Click" runat="server" />
                </div>
            </div>
        </nav>
        <br />
        <br />  
        <br />

        <div class="card">
            <div class="card-body">
                <h1>Lista de Usuários</h1>
                <br />
                <asp:Button ID="btnCadastroUsuario" OnClick="btnCadastroUsuario_Click" CssClass="btn btn-primary" Text="Cadastrar Usuário" runat="server" />

                <asp:GridView ID="gvUsuarios" runat="server" 
                    CssClass="table table-striped table-bordered"
                    AutoGenerateColumns="False" 
                    DataKeyNames="id_usuario"
                    OnRowCommand="gvUsuarios_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="id_usuario" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="nome" HeaderText="Nome" />
                        <asp:BoundField DataField="registro" HeaderText="Registro" />
                        <asp:BoundField DataField="telefone" HeaderText="Telefone" />
                        <asp:BoundField DataField="email" HeaderText="Email" />
                        <asp:BoundField DataField="nome_grupo" HeaderText="Grupo" />
                        <asp:BoundField DataField="ativo" HeaderText="Ativo" DataFormatString="{0:Sim;#;Não}" />

                        <asp:TemplateField HeaderText="Ações">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" 
                                    Text="Editar" CssClass="btn btn-warning btn-sm" 
                                    CommandName="Editar" CommandArgument='<%# Eval("id_usuario") %>' />

                                <asp:Button ID="btnExcluir" runat="server" 
                                    Text="Excluir" CssClass="btn btn-danger btn-sm"
                                    CommandName="Excluir" CommandArgument='<%# Eval("id_usuario") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <style>
            .card {
                border-radius:20px;
                border-color:lightblue;
                border-width:2px;
                margin: 40px auto;
                width: 95%;
                padding: 20px;
            }
            #btnCadastroUsuario {
                margin-bottom: 15px;
            }
        </style>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormularioAlta.aspx.cs" Inherits="ProjetoIntegrador.FormularioAlta" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Formulário de Alta Hospitalar</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .card { border-radius: 20px; border: 2px solid lightblue; padding: 20px; margin: 10px 0; }
        .input-group-text { background-color: lightblue; }
        .container {padding-top: 100px;}
    </style>
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
                     style="max-height: 95px;" />
            </div>
           
        </div>
    </div>
</nav>
        <div class="container mt-3">
            <div class="row">
                <div class="col-md-4">
                    <br />
                    <br /><br />

                    <h5>Internações do paciente</h5>
                    <asp:GridView ID="gvInternacoes" runat="server" AutoGenerateColumns="False"
                        CssClass="table table-striped" DataKeyNames="id_internacao"
                        OnSelectedIndexChanged="gvInternacoes_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="data_internacao" HeaderText="Internação" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Situação">
                                <ItemTemplate>
                                    <%# Eval("data_alta") == DBNull.Value ? "Internado" : "Alta" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" SelectText="Selecionar" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="col-md-8">
                    <br />
<br /><br />
                    <div class="card">

                        <h4>Formulário de Alta</h4>
                        <div class="mb-3 input-group">
                            <span class="input-group-text">Nome do Paciente</span>
                            <asp:Label ID="lblNome" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                        <div class="mb-3 input-group">
                            <span class="input-group-text">Idade</span>
                            <asp:Label ID="lblIdade" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                        <div class="mb-3 input-group">
                            <span class="input-group-text">Setor</span>
                            <asp:TextBox ID="txtSetor" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3 input-group">
                            <span class="input-group-text">Leito</span>
                            <asp:TextBox ID="txtLeito" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3 input-group">
                            <span class="input-group-text">Data da Internação</span>
                            <asp:TextBox ID="txtDataInternacao" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3 input-group">
                            <span class="input-group-text">Data da Alta</span>
                            <asp:TextBox ID="txtDataAlta" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="d-flex gap-2">
                            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success flex-grow-1" OnClick="btnSalvar_Click" />
                            <asp:Button ID="btnNovaInternacao" runat="server" Text="Nova Internação" CssClass="btn btn-secondary flex-grow-1" OnClick="btnNovaInternacao_Click" />
                            <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CssClass="btn btn-outline-primary flex-grow-1" OnClick="btnVoltar_Click" />
                        </div>
                        <br />
                        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

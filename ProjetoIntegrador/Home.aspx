<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ProjetoIntegrador.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-LN+7fdVzj6u52u30Kp6M/trliBMCMKTyK833zpbD+pXdCLuTusPj697FH4R/5mcr" crossorigin="anonymous">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js" integrity="sha384-ndDqU0Gzau9qJ1lfW4pNLlhNTkCfHzAVBReH9diLvGRem5+R9g2FzA8ZGN954O5Q" crossorigin="anonymous"></script>

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
                <!-- Direita: Botão Sair -->
                <div class="col-4 d-flex justify-content-end">
                    <asp:Button ID="btnSair" CssClass="btn btn-outline-danger" Text="Sair" OnClick="btnSair_Click1" runat="server" />
                </div>
            </div>
        </div>
    </nav>
        <br />
        <br />
        <br />  <br />
        

        <div class="card">
    <div class="card-body">
        <h1>Lista de Pacientes</h1>
        <br />
        <asp:Button ID="btnCadastroPac" OnClick="btnCadastroPac_Click" class="btn btn-primary" Text="Cadastrar Paciente" runat="server" />
        <div class="input-group mb-3">
            <asp:TextBox ID="txtPesquisa" runat="server" CssClass="form-control" placeholder="Pesquisar por nome ou CPF..." AutoPostBack="true" OnTextChanged="txtPesquisa_TextChanged" />
            <span class="input-group-text"><i class="bi bi-search"></i></span>
        </div>

        <asp:GridView ID="gvPacientes"
            runat="server"
            CssClass="table table-striped table-bordered"
            AllowPaging="True"
            PageSize="15"
            PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageText="&laquo;"
            PagerSettings-LastPageText="&raquo;"
            PagerSettings-PreviousPageText="&lsaquo;"
            PagerSettings-NextPageText="&rsaquo;"
            PagerStyle-CssClass="GridPager"
            OnPageIndexChanging="gvPacientes_PageIndexChanging"
            AutoGenerateColumns="False"
            DataKeyNames="id_paciente"
            OnRowCommand="gvPacientes_RowCommand">

            <Columns>
                <asp:BoundField DataField="id_paciente" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="nome" HeaderText="Nome" />
                <asp:BoundField DataField="nascimento" HeaderText="Nascimento" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="telefone" HeaderText="Telefone" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:BoundField DataField="endereco" HeaderText="Endereço" />
             

                <asp:TemplateField HeaderText="Ações">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server"
                            Text="Editar" CssClass="btn btn-warning btn-sm"
                            CommandName="Editar" CommandArgument='<%# Eval("id_paciente") %>' />


                        <asp:HyperLink ID="btnFormAlta" runat="server"
                            Text="Altas" CssClass="btn btn-secondary btn-sm"
                            NavigateUrl='<%# "FormularioAlta.aspx?id=" + Eval("id_paciente") %>' />

                        <asp:HyperLink ID="linkPTS" runat="server" 
                            Text="PTS" CssClass="btn btn-info btn-sm"
                            NavigateUrl='<%# "PTS.aspx?id=" + Eval("id_paciente") %>' />

                        <asp:HyperLink ID="btnPTA" runat="server"
                            Text="PTA" CssClass="btn btn-success btn-sm"
                            NavigateUrl='<%# "PTA.aspx?id=" + Eval("id_paciente") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                
        </asp:GridView>
    </div>
</div>

<style>
    .h3{
        margin: 10px;
        font-weight:650;
    }
    .card {
        border-radius:20px;
        border-color:lightblue;
        border-width:2px;
        margin: 40px auto;
        width: 95%;
        padding: 20px;
    }

    #btnCadastroPac {
        margin-bottom: 15px;
    }

</style>


        
    </form>
    <style>

 

        .divv{
            padding:15px;
        }

.GridPager {
    display: flex !important;
    justify-content: center;
    align-items: center;
    margin: 18px 0 10px 0;
}
.GridPager td {
    padding: 0 !important;
    margin: 0 !important;
    border: none !important;
    background: none !important;
}

/* botões quadrados com espacinho */
.GridPager a, .GridPager span, .aspNetPager a, .aspNetPager span {
    width: 38px !important;
    height: 38px !important;
    line-height: 38px !important;
    margin: 0 7px !important;
    border-radius: 8px !important;
    font-size: 1.09rem;
    color: #fff !important;
    background: #00a1e0 !important;
    border: none !important;
    display: block !important;
    text-align: center;
    font-weight: bold;
    box-shadow: none !important;
    transition: background 0.16s, box-shadow 0.12s;
}

/* Hover: escurece botão e faz "levitar" um pouco */
.GridPager a:hover, .aspNetPager a:hover {
    background: #0180b5 !important;
    color: #e9f9ff !important;
    box-shadow: 0 2px 8px #0080c045;
    text-decoration: none !important;
    z-index:1;
    position:relative;
}




    </style>
</body>
</html>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ConsultaPac.aspx.cs" Inherits="ProjetoIntegrador.ConsultarPaciente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br /><br /><br />

    <div class="card">
        <div class="card-body">
            <h1>Lista de Pacientes</h1>
            <br />

            <asp:GridView ID="gvPacientes" runat="server" 
                CssClass="table table-striped table-bordered" 
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
                    <asp:BoundField DataField="esf" HeaderText="ESF" />

                    <asp:TemplateField HeaderText="Ações">
                        <ItemTemplate>
                            <asp:Button ID="btnEditar" runat="server" 
                                Text="Editar" CssClass="btn btn-warning btn-sm" 
                                CommandName="Editar" CommandArgument='<%# Eval("id_paciente") %>' />

                            <asp:LinkButton ID="btnPTS" runat="server" 
                                Text="PTS" CssClass="btn btn-info btn-sm" 
                                CommandName="PTS" CommandArgument='<%# Eval("id_paciente") %>' />

                            <asp:Button ID="btnPTA" runat="server" 
                                Text="PTA" CssClass="btn btn-success btn-sm" 
                                CommandName="PTA" CommandArgument='<%# Eval("id_paciente") %>' />
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
    </style>
</asp:Content>

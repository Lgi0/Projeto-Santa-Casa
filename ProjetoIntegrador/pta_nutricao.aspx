<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pta_nutricao.aspx.cs" Inherits="ProjetoIntegrador.pta_nutricao" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plano Terapêutico para Alta Responsável - Nutrição</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body {
            background-color: lightblue;
            min-height: 100vh;
            padding: 20px;
        }
        .main-content { margin-top: 40px; }
        .sidebar-card {
            border-color: deepskyblue;
            border-width: 2px;
            border-radius: 15px;
            background: #fff;
        }
        .form-card {
            border-color: deepskyblue;
            border-width: 2px;
            border-radius: 20px;
            margin-bottom: 24px;
            background: #fff;
        }
        .card-body { padding-bottom: 0.5rem; }
        .fw-bold { font-weight: bold; }
        .titulo { display: flex; justify-content: space-between; align-items: center; }
        .line-1 { height: 1px; background: black; margin: 15px 0; }
        @media (max-width: 992px) { .main-content { flex-direction: column; } }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="container main-content">
        <div class="row">

            <!-- Sidebar Histórico -->
            <div class="col-lg-5 col-md-12 mb-3">
                <div class="card sidebar-card h-100">
                    <div class="card-body pb-2">
                        <h5 class="fw-bold mb-3">Histórico de PTA Nutrição</h5>
                        <asp:GridView 
                            ID="gvPtaNutricao"
                            runat="server"
                            AutoGenerateColumns="False"
                            DataKeyNames="id_pta_nutricao"
                            CssClass="table table-bordered table-hover"
                            Width="100%"
                            OnSelectedIndexChanged="gvPtaNutricao_SelectedIndexChanged"
                            AllowPaging="False"
                            ShowHeader="True">
                            <Columns>
                                <asp:BoundField ReadOnly="true" DataField="data_internacao" HeaderText="Internação" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField ReadOnly="true" DataField="setor" HeaderText="Setor" />
                                <asp:BoundField ReadOnly="true" DataField="leito" HeaderText="Leito" />
                                <asp:BoundField ReadOnly="true" DataField="status" HeaderText="Status" />
                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-info" Text="Selecionar" CommandName="Select" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Formulário -->
            <div class="col-lg-7 col-md-12">
                <div class="card form-card">
                    <div class="card-body">
                        <!-- Logo centralizada acima dos botões -->
<div class="d-flex flex-column align-items-center mb-3">
    <img src="https://examinadiagnosticos.com.br/wp-content/uploads/2021/03/santa-casa-de-adamantina.png"
         alt="Logo Santa Casa"
         style="max-height:200px;" class="mb-2" />
    <div>
        <asp:Button ID="btnEnfermagem" CssClass="btn btn-primary me-2 mb-2" Text="Enfermagem" runat="server" OnClick="btnEnfermagem_Click" />
        <asp:Button ID="btnFisioterapia" CssClass="btn btn-primary me-2 mb-2" Text="Fisioterapia" runat="server" OnClick="btnFisioterapia_Click" />
        <asp:Button ID="btnNutricao" CssClass="btn btn-primary me-2 mb-2" Text="Nutrição" runat="server" OnClick="btnNutricao_Click" />
        <asp:Button ID="btnPsicologia" CssClass="btn btn-primary me-2 mb-2" Text="Psicologia" runat="server" OnClick="btnPsicologia_Click" />
        <asp:Button ID="btnSocial" CssClass="btn btn-primary mb-2" Text="Serviço Social" runat="server" OnClick="btnSocial_Click" />
    </div>
</div>

                        <div class="titulo mb-3">
                            <h2 class="fw-bold">Plano Terapêutico para Alta Responsável - Nutrição</h2>
                            <asp:Button ID="btnVoltar" Text="Voltar" CssClass="btn btn-danger" runat="server" OnClick="btnVoltar_Click"/>
                        </div>

                        <!-- Dados do Paciente -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label">Nome do paciente:</label>
                                <asp:TextBox ID="txtNome" ReadOnly="true" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Idade:</label>
                                <asp:TextBox ID="txtIdade" ReadOnly="true" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Setor:</label>
                                <asp:TextBox ID="txtSetor" ReadOnly="true" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Leito:</label>
                                <asp:TextBox ID="txtLeito" ReadOnly="true" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label">Médico:</label>
                                <asp:TextBox ID="txtMedico"  runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">CRM:</label>
                                <asp:TextBox ID="txtCRM" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label">Data da Internação:</label>
                                <asp:TextBox ID="txtInternacao" runat="server" CssClass="form-control" TextMode="Date" ReadOnly="true" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Data da Alta:</label>
                                <asp:TextBox ID="txtAlta" ReadOnly="true" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                        </div>
                        <div class="mb-3">
                            <label class="form-label">HD:</label>
                            <asp:TextBox ID="txtHD" runat="server" CssClass="form-control" />
                        </div>

                        <!-- Condições -->
                        <div class="mb-3">
                            <label class="form-label">Paciente portador de:</label><br />
                            <asp:CheckBox ID="chkDiabetes" runat="server" Text="Diabetes" /> &nbsp;
                            <asp:CheckBox ID="chkHAS" runat="server" Text="HAS" /> &nbsp;
                            <asp:CheckBox ID="chkClinico" runat="server" Text="Clínico" /> &nbsp;
                            <asp:CheckBox ID="chkCirurgico" runat="server" Text="Cirúrgico" /> <br />
                            <label>Outros:</label>
                            <asp:TextBox ID="txtOutros" runat="server" CssClass="form-control mt-1" />
                        </div>

                        <!-- Indicadores Nutricionais -->
                        <div class="line-1"></div>
                        <label class="form-label">Indicadores de necessidade nutricional:</label><br />
                        <asp:CheckBox ID="chkInapetencia" runat="server" Text="Inapetência" /><br />
                        <asp:CheckBox ID="chkDesnutricao" runat="server" Text="Desnutrição" /><br />
                        <asp:CheckBox ID="chkRisco" runat="server" Text="Risco de Desnutrição" /><br />
                        <asp:CheckBox ID="chkSobrepeso" runat="server" Text="Sobrepeso" /><br />
                        <asp:CheckBox ID="chkObesidade" runat="server" Text="Obesidade" /><br />
                        <asp:CheckBox ID="chkNPT" runat="server" Text="NPT" /><br />
                        <label>Outros:</label>
                        <asp:TextBox ID="txtOutrosIndicadores" runat="server" CssClass="form-control mb-3" />

                        <!-- Alta com necessidades -->
                        <div class="line-1"></div>
                        <label class="form-label">Paciente terá alta fazendo uso ou necessidade de:</label><br />
                        <asp:CheckBox ID="chkOral" runat="server" Text="Oral" /><br />
                        <asp:CheckBox ID="chkOralAssistida" runat="server" Text="Oral assistida" /><br />
                        <asp:CheckBox ID="chkEnteral" runat="server" Text="Enteral" /><br />
                        <label>Outros:</label>
                        <asp:TextBox ID="txtOutrosAlta" runat="server" CssClass="form-control mb-3" />

                        <!-- Orientações -->
                        <div class="line-1"></div>
                        <label class="form-label">Orientações realizadas para:</label><br />
                        <asp:CheckBox ID="chkPaciente" runat="server" Text="Paciente" /> &nbsp;
                        <asp:CheckBox ID="chkFamiliar" runat="server" Text="Familiar/Cuidador" /><br />
                        <label>Quais:</label>
                        <asp:TextBox ID="txtOrientacoes" runat="server" CssClass="form-control mt-1" />

                        <!-- Ações Desenvolvidas -->
                        <div class="line-1"></div>
                        <label class="form-label">Ações desenvolvidas:</label><br />
                        <asp:CheckBox ID="chkDietaEnteral" runat="server" Text="Orientação para administração de dieta enteral" /><br />
                        <asp:CheckBox ID="chkEncaminhamento" runat="server" Text="Encaminhamento" /><br />
                        <asp:CheckBox ID="chkRedeBasica" runat="server" Text="Contato com a rede básica" /><br />
                        <label>Outros:</label>
                        <asp:TextBox ID="txtOutrosAcoes" runat="server" CssClass="form-control mb-3" />

                        <!-- Descrição e Dados -->
                        <div class="line-1"></div>
                        <div class="mb-3">
                            <label class="form-label">Descrição do caso clínico:</label>
                            <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Dados Iniciais:</label>
                            <asp:TextBox ID="txtIniciais" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Dados da Internação:</label>
                            <asp:TextBox ID="txtInternacaoDados" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Dados de Alta:</label>
                            <asp:TextBox ID="txtDadosAlta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>

                        <div class="d-flex justify-content-end mb-2">
                            <asp:Button 
                                ID="btnSalvar" 
                                runat="server" 
                                Text="Salvar" 
                                CssClass="btn btn-success fw-bold px-5" 
                                OnClick="btnSalvar_Click"/>

                            <asp:Button 
                                ID="btnGerarPDF" 
                                runat="server" 
                                CssClass="btn btn-secondary fw-bold px-5 ms-2" 
                                Text="Gerar PDF" 
                                OnClick="btnGerarPDF_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</form>
</body>
</html>

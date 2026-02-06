<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pta_fisioterapia.aspx.cs" Inherits="ProjetoIntegrador.pta_fisioterapia" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plano Terapêutico para Alta Responsável - Fisioterapia</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body {
            background-color: lightblue;
            min-height: 100vh;
            padding: 20px;
        }
        .main-content {
            margin-top: 40px;
        }
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
        .card-body {
            padding-bottom: 0.5rem;
        }
        .fw-bold { font-weight: bold; }
        .titulo { display: flex; justify-content: space-between; align-items: center; }
        .line-1 { height: 1px; background: black; margin: 15px 0; }
        @media (max-width: 992px) {
            .main-content { flex-direction: column; }
        }
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
                        <h5 class="fw-bold mb-3">Histórico de PTA Fisioterapia</h5>
                        <asp:GridView 
                            ID="gvPtaFisio"
                            runat="server"
                            AutoGenerateColumns="False"
                            DataKeyNames="id_pta_fisioterapia"
                            CssClass="table table-bordered table-hover"
                            Width="100%"
                            OnSelectedIndexChanged="gvPtaFisio_SelectedIndexChanged"
                            AllowPaging="False"
                            ShowHeader="True">
                            <Columns>
                                <asp:BoundField DataField="data_internacao"  HeaderText="Internação" DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                <asp:BoundField DataField="setor" HeaderText="Setor" ReadOnly="true" />
                                <asp:BoundField DataField="leito" HeaderText="Leito" ReadOnly="true" />
                                <asp:BoundField DataField="status" HeaderText="Situação" ReadOnly="true" />
                                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn btn-info" Text="Selecionar" CommandName="Select" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Formulário PTA -->
            <div class="col-lg-7 col-md-12">
                <div class="card form-card">
                    <div class="card-body">
                        <!-- Navegação de sessões multiprofissionais -->
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

                        <!-- Cabeçalho -->
                        <div class="titulo mb-3">
                            <h2 class="fw-bold">Plano Terapêutico para Alta Responsável - Fisioterapia</h2>
                            <asp:Button ID="btnVoltar" Text="Voltar" CssClass="btn btn-danger" runat="server" OnClick="btnVoltar_Click"/>
                        </div>

                        <!-- Dados do Paciente -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label">Nome do paciente:</label>
                                <asp:TextBox ReadOnly="true" ID="txtNome" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Idade:</label>
                                <asp:TextBox ReadOnly="true" ID="txtIdade" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Setor:</label>
                                <asp:TextBox ReadOnly="true" ID="txtSetor" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Leito:</label>
                                <asp:TextBox ReadOnly="true" ID="txtLeito" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label">Médico:</label>
                                <asp:TextBox ID="txtMedico" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">CRM:</label>
                                <asp:TextBox ID="txtCRM" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label class="form-label">Data da Internação:</label>
                                <asp:TextBox ID="txtInternacao" runat="server" CssClass="form-control" ReadOnly="true" TextMode="Date" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Data da Alta:</label> 
                                <asp:TextBox ID="txtAlta" runat="server" ReadOnly="true" CssClass="form-control" TextMode="Date"  />
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

                        <!-- Necessidade de Fisioterapia -->
                        <div class="line-1"></div>
                        <label class="form-label">Necessidade de Fisioterapia:</label><br />
                        <asp:CheckBox ID="chkMotora" runat="server" Text="Motora" /><br />
                        <asp:CheckBox ID="chkRespiratoria" runat="server" Text="Respiratória" /><br />
                        <asp:CheckBox ID="chkDeambulacao" runat="server" Text="Deambulação precoce" /><br />

                        <div class="mt-2">
                            <label class="form-label">Tosse:</label><br />
                            <asp:CheckBox ID="chkProdutiva" runat="server" Text="Produtiva" /> &nbsp;
                            <asp:CheckBox ID="chkImprodutiva" runat="server" Text="Improdutiva" /><br />
                            <asp:CheckBox ID="chkAspiracao" runat="server" Text="Aspiração Periódica" />
                        </div>

                        <!-- Escala de Dependência -->
                        <div class="line-1"></div>
                        <h5 class="fw-bold">Níveis de dependência/independência</h5>
                        <p>Independência = 1 ponto &nbsp;&nbsp; Dependência = 0 pontos</p>

                        <table class="table table-bordered">
                            <thead class="table-light">
                                <tr>
                                    <th>Atividade</th>
                                    <th>Pontos</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Banhar-se</td>
                                    <td><asp:TextBox ID="txtBanhar" runat="server" CssClass="form-control" /></td>
                                </tr>
                                <tr>
                                    <td>Vestir-se</td>
                                    <td><asp:TextBox ID="txtVestir" runat="server" CssClass="form-control" /></td>
                                </tr>
                                <tr>
                                    <td>Ir ao Banheiro</td>
                                    <td><asp:TextBox ID="txtBanheiro" runat="server" CssClass="form-control" /></td>
                                </tr>
                                <tr>
                                    <td>Transferência</td>
                                    <td><asp:TextBox ID="txtTransferencia" runat="server" CssClass="form-control" /></td>
                                </tr>
                                <tr>
                                    <td>Continência</td>
                                    <td><asp:TextBox ID="txtContinencia" runat="server" CssClass="form-control" /></td>
                                </tr>
                                <tr>
                                    <td>Alimentação</td>
                                    <td><asp:TextBox ID="txtAlimentacao" runat="server" CssClass="form-control" /></td>
                                </tr>
                                <tr>
                                    <td class="fw-bold">Total</td>
                                    <td><asp:TextBox ID="txtTotal" runat="server" CssClass="form-control fw-bold" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <p><strong>6 = Independente | 4 = Dependência Moderada | 2 ou menos = Muito dependente</strong></p>

                        <!-- Alta com necessidades -->
                        <div class="line-1"></div>
                        <label class="form-label">Paciente terá alta necessitando de:</label><br />
                        <asp:CheckBox ID="chkAltaMotora" runat="server" Text="Fisioterapia Motora" /><br />
                        <asp:CheckBox ID="chkAltaRespiratoria" runat="server" Text="Respiratória" /><br />
                        <label>Outro:</label>
                        <asp:TextBox ID="txtOutroAlta" runat="server" CssClass="form-control mb-3" />

                        <!-- Ações Realizadas -->
                        <div class="line-1"></div>
                        <label class="form-label">Ações Realizadas:</label><br />
                        <asp:CheckBox ID="chkEncaminhamentos" runat="server" Text="Encaminhamentos" /> &nbsp;
                        <asp:CheckBox ID="chkOrientacoes" runat="server" Text="Orientações" /><br />
                        <label>Quais:</label>
                        <asp:TextBox ID="txtQuais" runat="server" CssClass="form-control mt-1" />

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

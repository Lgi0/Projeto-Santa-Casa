<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="psico.aspx.cs" Inherits="ProjetoIntegrador.psico" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plano Terapêutico para Alta Responsável - Psicologia</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body {
            background-color: lightblue;
            display: flex;
            justify-content: center;
            align-items: flex-start;
            min-height: 100vh;
            padding: 20px;
        }
        .card {
            max-width: 1000px;
            width: 100%;
        }
        .titulo {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .line-1 {
            height: 1px;
            background: black;
            margin: 15px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <div class="card" style="position: absolute;
             left: 50%;
             top: 60px;
             transform: translate(-50%);
             border-color: deepskyblue;
             border-style: ridge;
             padding: 5px;
             border-width: medium;
             border-radius: 20px;
             width:fit-content;">
        <div class="card-body">
           <asp:Button ID="btnEnfermagem"  class="btn btn-primary" Text="Enfermagem" runat="server" OnClick="btnEnfermagem_Click" />
            <asp:Button ID="btnFisioterapia"  class="btn btn-primary" Text="Fisioterapia" runat="server" OnClick="btnFisioterapia_Click" />
            <asp:Button ID="btnNutricao"  class="btn btn-primary" Text="Nutrição" runat="server" OnClick="btnNutricao_Click" />
            <asp:Button ID="btnPsicologia"  class="btn btn-primary" Text="Psicologia" runat="server" OnClick="btnPsicologia_Click" />
            <asp:Button ID="btnSocial"  class="btn btn-primary" Text="Serviço Social" runat="server" OnClick="btnSocial_Click" />
            </div>
        </div>
        <div class="card" style="margin-top:130px;">
            <div class="card-body">

                <!-- Cabeçalho -->
                <div class="titulo mb-3">
                    <h2 class="fw-bold">Plano Terapêutico para Alta Responsável - Psicologia</h2>
                    <asp:Button ID="btnVoltar" Text="Voltar" class="btn btn-danger" runat="server" OnClick="btnVoltar_Click"/>
                </div>

                <!-- Dados do Paciente -->
                <div class="row mb-3">
                    <div class="col-md-6">
                        <label class="form-label">Nome do paciente:</label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label">Idade:</label>
                        <asp:TextBox ID="txtIdade" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label">Setor:</label>
                        <asp:TextBox ID="txtSetor" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label class="form-label">Leito:</label>
                        <asp:TextBox ID="txtLeito" runat="server" CssClass="form-control" />
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
                        <asp:TextBox ID="txtInternacao" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>
                    <div class="col-md-6">
                        <label class="form-label">Data da Alta:</label>
                        <asp:TextBox ID="txtAlta" runat="server" CssClass="form-control" TextMode="Date" />
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

                <!-- Indicadores Psicológicos -->
                <div class="line-1"></div>
                <label class="form-label">Indicadores de necessidade de suporte psicológico:</label><br />
                <asp:CheckBox ID="chkAlteracoes" runat="server" Text="Alterações psicoemocionais" /><br />
                <asp:CheckBox ID="chkBaixaAdesao" runat="server" Text="Baixa adesão ao tratamento" /><br />
                <asp:CheckBox ID="chkSuicidio" runat="server" Text="Tentativa de suicídio" /><br />
                <asp:CheckBox ID="chkSuporte" runat="server" Text="Suporte emocional para patologias confirmadas" /><br />
                <asp:CheckBox ID="chkBaixoNivel" runat="server" Text="Baixo nível de compreensão processo doença x saúde" /><br />
                <label>Outros:</label>
                <asp:TextBox ID="txtOutrosIndicadores" runat="server" CssClass="form-control mb-3" />

                <!-- Ações Desenvolvidas -->
                <div class="line-1"></div>
                <label class="form-label">Ações desenvolvidas:</label><br />
                <asp:CheckBox ID="chkIntervencaoPaciente" runat="server" Text="Intervenções com o paciente" /><br />
                <asp:CheckBox ID="chkIntervencaoFamiliares" runat="server" Text="Intervenções com os familiares" /><br />
                <asp:CheckBox ID="chkOrientacaoFamilia" runat="server" Text="Orientação aos familiares" />

                <div class="mt-2">
                    <label class="form-label">Encaminhamento para:</label>
                    <asp:TextBox ID="txtEncaminhamento" runat="server" CssClass="form-control" />
                </div>

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

                <!-- Botão -->
                <div class="d-flex justify-content-end">
                    <button type="submit" class="btn btn-success fw-bold px-5">Salvar</button>
                </div>

            </div>
        </div>
    </form>
</body>
</html>

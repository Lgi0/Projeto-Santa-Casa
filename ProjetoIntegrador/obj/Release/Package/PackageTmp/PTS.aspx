<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PTS.aspx.cs" Inherits="ProjetoIntegrador.WebForm1" EnableEventValidation="false" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>PTS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body>

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

        .nome {
            font-weight: 700;
        }

        .titulo {
            display: flex;
            justify-content: space-between;
        }

        .divdiv {
            display: flex;
            justify-content: flex-start;
            gap: 25px;
        }

        .line-1 {
            height: 1px;
            background: black;
            margin: 15px;
        }
    </style>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfIdPaciente" runat="server" />
        <asp:HiddenField ID="hfIdPTS" runat="server" />

        <asp:HiddenField ID="hfNomePaciente" runat="server" />
    <asp:HiddenField ID="hfIdadePaciente" runat="server" />
    <asp:HiddenField ID="hfEnderecoPaciente" runat="server" />
    <asp:HiddenField ID="hfESFPaciente" runat="server" />
   
<div class="container mt-4">
  <div class="row">
    <!-- Histórico lateral -->
    <div class="col-lg-4 col-md-12 mb-3">
      <div class="card" style="border-color:deepskyblue;border-width:2px;border-radius:15px;">
        <div class="card-body pb-2">
          <h5 class="fw-bold mb-3">Histórico de PTS</h5>
          <asp:GridView 
            ID="gvPts"
            runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="id_pts"
            CssClass="table table-bordered table-hover"
            Width="100%"
            OnSelectedIndexChanged="gvPts_SelectedIndexChanged"
            AllowPaging="False"
            ShowHeader="True">
            <Columns>
              <asp:BoundField DataField="data_criacao" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
              <asp:BoundField DataField="enfermeiro" HeaderText="Enfermeiro" />
              <asp:BoundField DataField="status" HeaderText="Status" />
              <asp:ButtonField ButtonType="Button" Text="Selecionar" CommandName="Select" />
            </Columns>
          </asp:GridView>
        </div>
      </div>
    </div>
    <!-- Formulário principal -->
    <div class="col-lg-8 col-md-12">
      <div class="card" style="max-width: 1000px; width: 850px; border-color: deepskyblue; border-width: 2px;">
        <div class="card-body">

                <div class="text-center mb-3">
                    <img src="https://examinadiagnosticos.com.br/wp-content/uploads/2021/03/santa-casa-de-adamantina.png"
                         alt="Logo Santa Casa"
                         class="img-fluid"
                         style="max-height: 150px;" />
                </div>

                <div class="titulo">
                    <h1 class="nome">Plano Terapêutico Singular</h1>
                    <asp:Button ID="btnVoltar" Text="Voltar" style="width: 90px; height: 45px; font-weight: 700" type="button" class="btn btn-danger" runat="server" OnClick="btnVoltar_Click"/>
                </div>

                <br />
                <div class="row mb-3">
                    <div class="col-lg-6 col-md-12">
                        <label for="inputPaciente" class="form-label">Paciente:</label>
                        <asp:TextBox style="width: 100%;" type="text" disabled="true" class="form-control" id="inputPaciente" runat="server" />
                    </div>
                    <div class="col-lg-2 col-md-6">
                        <label for="inputIdade" class="form-label">Idade:</label>
                        <asp:TextBox style="width: 100%;" type="text" disabled="true" class="form-control" id="inputIdade" runat="server" />
                    </div>
                    <div class="col-lg-4 col-md-6">
                        <label for="inputESF" class="form-label">ESF:</label>
                        <asp:TextBox style="width: 100%;" type="text" disabled="true" class="form-control" id="inputESF" runat="server" />
                    </div>
                </div>
                <div class="mb-3">
                    <label for="inputEndereco" class="form-label">Endereço:</label>
                    <asp:TextBox  type="text" disabled="true" class="form-control" id="inputEndereco" placeholder="Rua Sirlene Rodrigues de Castro, 518"  runat="server" />

                </div>

                <div class="mb-3">
                    <label for="inputEnfermeiro" class="form-label">Enfermeiro:</label>
                    <asp:TextBox ID="inputEnfermeiro" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="inputMedica" class="form-label">Médica:</label>
                    <asp:TextBox ID="inputMedica" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="inputNutri" class="form-label">Nutricionista:</label>
                    <asp:TextBox ID="inputNutri" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="inputFisio" class="form-label">Fisioterapeuta:</label>
                    <asp:TextBox ID="inputFisio" runat="server" CssClass="form-control" />
                </div>

                <div class="mb-3">
                    <label for="inputPsico" class="form-label">Psicóloga:</label>
                    <asp:TextBox ID="inputPsico" runat="server" CssClass="form-control" />
                </div>


                <div class="mb-3">
                    <label  for="inputContinuo" class="form-label">Medicamentos de uso contínuo:</label>
                    <asp:TextBox ID="inputContinuo" runat="server" CssClass="form-control" />
                    
                </div>

                <div class="mb-3">
                    <label for="inputPregressa" class="form-label">História Pregressa:</label>
                    <asp:TextBox ID="inputPregressa" runat="server" CssClass="form-control" />
                </div>

                <div class="line-1"></div>

                <div class="mb-3">
                    <a class="btn btn-primary w-100 text-start"
                        data-bs-toggle="collapse"
                        href="#collapseEnfermagem"
                        role="button"
                        aria-expanded="false"
                        aria-controls="collapseEnfermagem">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-down-right-square-fill" viewBox="0 0 16 16">
                            <path d="M14 16a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2zM5.904 5.197 10 9.293V6.525a.5.5 0 0 1 1 0V10.5a.5.5 0 0 1-.5.5H6.525a.5.5 0 0 1 0-1h2.768L5.197 5.904a.5.5 0 0 1 .707-.707" />
                        </svg>

                        Enfermagem
                        </a>
                    </div>
                <div class="collapse" id="collapseEnfermagem">
                    <div class="card card-body mt-2">
                        <label for="inputEnfermagem" class="form-label">Enfermagem:</label>
                        <asp:TextBox ID="inputEnfermagem" runat="server" CssClass="form-control"  />
                        
                    </div>
                </div>

                <div class="line-1"></div>

                <div class="mb-3">
                    <a class="btn btn-primary w-100 text-start"
                        data-bs-toggle="collapse"
                        href="#collapseFisioterapia"
                        role="button"
                        aria-expanded="false"
                        aria-controls="collapseFisioterapia">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-down-right-square-fill" viewBox="0 0 16 16">
                            <path d="M14 16a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2zM5.904 5.197 10 9.293V6.525a.5.5 0 0 1 1 0V10.5a.5.5 0 0 1-.5.5H6.525a.5.5 0 0 1 0-1h2.768L5.197 5.904a.5.5 0 0 1 .707-.707" />
                        </svg>
                        Fisioterapia
                    </a>
                </div>

                <div class="collapse" id="collapseFisioterapia">
                    <div class="card card-body mt-2">
                        <label for="inputFisioterapia" class="form-label">Fisioterapia:</label>
                        <asp:TextBox ID="inputFisioterapia" runat="server" CssClass="form-control" />

                        <label for="inputPA" class="form-label mt-2">PA:</label>
                        <asp:TextBox ID="inputPA" runat="server" CssClass="form-control"  />

                        <label for="inputInspecao" class="form-label mt-2">Inspeção:</label>
                        <asp:TextBox ID="inputInspecao" runat="server" CssClass="form-control"  />

                        <label for="inputAvaliacao" class="form-label mt-2">Avaliação Física:</label>
                        <asp:TextBox ID="inputAvaliacao" runat="server" CssClass="form-control" />

                        <label for="inputGrau" class="form-label mt-2">Grau de Mobilidade:</label>
                        <asp:TextBox ID="inputGrau" runat="server" CssClass="form-control" />

                        <label for="inputForca" class="form-label mt-2">Força e Sensibilidade:</label>
                        <asp:TextBox ID="inputForca" runat="server" CssClass="form-control" />

                        <label for="inputDependencia" class="form-label mt-2">Nível de Dependência:</label>
                        <asp:TextBox ID="inputDependencia" runat="server" CssClass="form-control"  />

                    </div>
                </div>

                <div class="line-1"></div>
                <div class="mb-3">
                    <a class="btn btn-primary w-100 text-start"
                        data-bs-toggle="collapse"
                        href="#collapseNutricao"
                        role="button"
                        aria-expanded="false"
                        aria-controls="collapseNutricao">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-down-right-square-fill" viewBox="0 0 16 16">
                            <path d="M14 16a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2zM5.904 5.197 10 9.293V6.525a.5.5 0 0 1 1 0V10.5a.5.5 0 0 1-.5.5H6.525a.5.5 0 0 1 0-1h2.768L5.197 5.904a.5.5 0 0 1 .707-.707" />
                        </svg>
                        Nutrição
                    </a>
                </div>

                <div class="collapse" id="collapseNutricao">
                    <div class="card card-body mt-2">
                        <label for="inputNutricao" class="form-label">Nutrição:</label>
                        <asp:TextBox ID="inputNutricao" runat="server" CssClass="form-control"  />
                    </div>
                </div>







                <div class="line-1"></div>

<div class="container mt-4">

    <!-- ========== AÇÕES DE CURTO PRAZO ========== -->
    <div class="row border">
        <div class="col-3 border p-3 fw-bold">
            Ações de Curto Prazo<br />
            Início imediato
        </div>
        <div class="col-9 border p-3">

            <!-- Enfermagem -->
            <a class="btn btn-outline-primary w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#curtoEnfermagem">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-hospital" viewBox="0 0 16 16">
                    <path d="M8.5 5.034v1.1l.953-.55.5.867L9 7l.953.55-.5.866-.953-.55v1.1h-1v-1.1l-.953.55-.5-.866L7 7l-.953-.55.5-.866.953.55v-1.1zM13.25 9a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zM13 11.25a.25.25 0 0 1 .25-.25h.5a.25.25 0 0 1 .25.25v.5a.25.25 0 0 1-.25.25h-.5a.25.25 0 0 1-.25-.25zm.25 1.75a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zm-11-4a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5A.25.25 0 0 0 3 9.75v-.5A.25.25 0 0 0 2.75 9zm0 2a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zM2 13.25a.25.25 0 0 1 .25-.25h.5a.25.25 0 0 1 .25.25v.5a.25.25 0 0 1-.25.25h-.5a.25.25 0 0 1-.25-.25z"/>
                    <path d="M5 1a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v1a1 1 0 0 1 1 1v4h3a1 1 0 0 1 1 1v7a1 1 0 0 1-1 1H1a1 1 0 0 1-1-1V8a1 1 0 0 1 1-1h3V3a1 1 0 0 1 1-1zm2 14h2v-3H7zm3 0h1V3H5v12h1v-3a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1zm0-14H6v1h4zm2 7v7h3V8zm-8 7V8H1v7z"/>
                </svg> Enfermagem
            </a>
            <div class="collapse" id="curtoEnfermagem">
                <asp:TextBox ID="txtCurtoEnfermagem" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de enfermagem (curto prazo)..."></asp:TextBox>
            </div>

            <!-- Fisioterapia -->
            <a class="btn btn-outline-primary w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#curtoFisio">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-standing" viewBox="0 0 16 16">
                    <path d="M8 3a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3M6 6.75v8.5a.75.75 0 0 0 1.5 0V10.5a.5.5 0 0 1 1 0v4.75a.75.75 0 0 0 1.5 0v-8.5a.25.25 0 1 1 .5 0v2.5a.75.75 0 0 0 1.5 0V6.5a3 3 0 0 0-3-3H7a3 3 0 0 0-3 3v2.75a.75.75 0 0 0 1.5 0v-2.5a.25.25 0 0 1 .5 0"/>
                </svg> Fisioterapia
            </a>
            <div class="collapse" id="curtoFisio">
                <asp:TextBox ID="txtCurtoFisio" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de fisioterapia (curto prazo)..."></asp:TextBox>
            </div>

            <!-- Nutricionista -->
            <a class="btn btn-outline-primary w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#curtoNutri">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-fork-knife" viewBox="0 0 16 16">
                    <path d="M13 .5c0-.276-.226-.506-.498-.465-1.703.257-2.94 2.012-3 8.462a.5.5 0 0 0 .498.5c.56.01 1 .13 1 1.003v5.5a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5zM4.25 0a.25.25 0 0 1 .25.25v5.122a.128.128 0 0 0 .256.006l.233-5.14A.25.25 0 0 1 5.24 0h.522a.25.25 0 0 1 .25.238l.233 5.14a.128.128 0 0 0 .256-.006V.25A.25.25 0 0 1 6.75 0h.29a.5.5 0 0 1 .498.458l.423 5.07a1.69 1.69 0 0 1-1.059 1.711l-.053.022a.92.92 0 0 0-.58.884L6.47 15a.971.971 0 1 1-1.942 0l.202-6.855a.92.92 0 0 0-.58-.884l-.053-.022a1.69 1.69 0 0 1-1.059-1.712L3.462.458A.5.5 0 0 1 3.96 0z"/>
                </svg> Nutricionista
            </a>
            <div class="collapse" id="curtoNutri">
                <asp:TextBox ID="txtCurtoNutri" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de nutrição (curto prazo)..."></asp:TextBox>
            </div>

        </div>
    </div>

    <!-- ========== AÇÕES DE MÉDIO PRAZO ========== -->
    <div class="row border mt-3">
        <div class="col-3 border p-3 fw-bold">
            Ações de Médio Prazo
        </div>
        <div class="col-9 border p-3">

            <!-- Enfermagem -->
            <a class="btn btn-outline-success w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#medioEnfermagem">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-hospital" viewBox="0 0 16 16">
                    <path d="M8.5 5.034v1.1l.953-.55.5.867L9 7l.953.55-.5.866-.953-.55v1.1h-1v-1.1l-.953.55-.5-.866L7 7l-.953-.55.5-.866.953.55v-1.1zM13.25 9a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zM13 11.25a.25.25 0 0 1 .25-.25h.5a.25.25 0 0 1 .25.25v.5a.25.25 0 0 1-.25.25h-.5a.25.25 0 0 1-.25-.25zm.25 1.75a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zm-11-4a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5A.25.25 0 0 0 3 9.75v-.5A.25.25 0 0 0 2.75 9zm0 2a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zM2 13.25a.25.25 0 0 1 .25-.25h.5a.25.25 0 0 1 .25.25v.5a.25.25 0 0 1-.25.25h-.5a.25.25 0 0 1-.25-.25z"/>
                    <path d="M5 1a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v1a1 1 0 0 1 1 1v4h3a1 1 0 0 1 1 1v7a1 1 0 0 1-1 1H1a1 1 0 0 1-1-1V8a1 1 0 0 1 1-1h3V3a1 1 0 0 1 1-1zm2 14h2v-3H7zm3 0h1V3H5v12h1v-3a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1zm0-14H6v1h4zm2 7v7h3V8zm-8 7V8H1v7z"/>
                </svg> Enfermagem
            </a>
            <div class="collapse" id="medioEnfermagem">
                <asp:TextBox ID="txtMedioEnfermagem" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de enfermagem (médio prazo)..."></asp:TextBox>
            </div>

            <!-- Fisioterapia -->
            <a class="btn btn-outline-success w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#medioFisio">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-standing" viewBox="0 0 16 16">
                    <path d="M8 3a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3M6 6.75v8.5a.75.75 0 0 0 1.5 0V10.5a.5.5 0 0 1 1 0v4.75a.75.75 0 0 0 1.5 0v-8.5a.25.25 0 1 1 .5 0v2.5a.75.75 0 0 0 1.5 0V6.5a3 3 0 0 0-3-3H7a3 3 0 0 0-3 3v2.75a.75.75 0 0 0 1.5 0v-2.5a.25.25 0 0 1 .5 0"/>
                </svg> Fisioterapia
            </a>
            <div class="collapse" id="medioFisio">
                <asp:TextBox ID="txtMedioFisio" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de fisioterapia (médio prazo)..."></asp:TextBox>
            </div>

            <!-- Nutricionista -->
            <a class="btn btn-outline-success w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#medioNutri">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-fork-knife" viewBox="0 0 16 16">
                    <path d="M13 .5c0-.276-.226-.506-.498-.465-1.703.257-2.94 2.012-3 8.462a.5.5 0 0 0 .498.5c.56.01 1 .13 1 1.003v5.5a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5zM4.25 0a.25.25 0 0 1 .25.25v5.122a.128.128 0 0 0 .256.006l.233-5.14A.25.25 0 0 1 5.24 0h.522a.25.25 0 0 1 .25.238l.233 5.14a.128.128 0 0 0 .256-.006V.25A.25.25 0 0 1 6.75 0h.29a.5.5 0 0 1 .498.458l.423 5.07a1.69 1.69 0 0 1-1.059 1.711l-.053.022a.92.92 0 0 0-.58.884L6.47 15a.971.971 0 1 1-1.942 0l.202-6.855a.92.92 0 0 0-.58-.884l-.053-.022a1.69 1.69 0 0 1-1.059-1.712L3.462.458A.5.5 0 0 1 3.96 0z"/>
                </svg> Nutricionista
            </a>
            <div class="collapse" id="medioNutri">
                <asp:TextBox ID="txtMedioNutri" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de nutrição (médio prazo)..."></asp:TextBox>
            </div>

        </div>
    </div>

    <!-- ========== AÇÕES DE LONGO PRAZO ========== -->
    <div class="row border mt-3">
        <div class="col-3 border p-3 fw-bold">
            Ações de Longo Prazo
        </div>
        <div class="col-9 border p-3">

            <!-- Enfermagem -->
            <a class="btn btn-outline-danger w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#longoEnfermagem">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-hospital" viewBox="0 0 16 16">
                    <path d="M8.5 5.034v1.1l.953-.55.5.867L9 7l.953.55-.5.866-.953-.55v1.1h-1v-1.1l-.953.55-.5-.866L7 7l-.953-.55.5-.866.953.55v-1.1zM13.25 9a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zM13 11.25a.25.25 0 0 1 .25-.25h.5a.25.25 0 0 1 .25.25v.5a.25.25 0 0 1-.25.25h-.5a.25.25 0 0 1-.25-.25zm.25 1.75a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zm-11-4a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5A.25.25 0 0 0 3 9.75v-.5A.25.25 0 0 0 2.75 9zm0 2a.25.25 0 0 0-.25.25v.5c0 .138.112.25.25.25h.5a.25.25 0 0 0 .25-.25v-.5a.25.25 0 0 0-.25-.25zM2 13.25a.25.25 0 0 1 .25-.25h.5a.25.25 0 0 1 .25.25v.5a.25.25 0 0 1-.25.25h-.5a.25.25 0 0 1-.25-.25z"/>
                    <path d="M5 1a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v1a1 1 0 0 1 1 1v4h3a1 1 0 0 1 1 1v7a1 1 0 0 1-1 1H1a1 1 0 0 1-1-1V8a1 1 0 0 1 1-1h3V3a1 1 0 0 1 1-1zm2 14h2v-3H7zm3 0h1V3H5v12h1v-3a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1zm0-14H6v1h4zm2 7v7h3V8zm-8 7V8H1v7z"/>
                </svg> Enfermagem
            </a>
            <div class="collapse" id="longoEnfermagem">
                <asp:TextBox ID="txtLongoEnfermagem" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de enfermagem (longo prazo)..."></asp:TextBox>
            </div>

            <!-- Fisioterapia -->
            <a class="btn btn-outline-danger w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#longoFisio">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-standing" viewBox="0 0 16 16">
                    <path d="M8 3a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3M6 6.75v8.5a.75.75 0 0 0 1.5 0V10.5a.5.5 0 0 1 1 0v4.75a.75.75 0 0 0 1.5 0v-8.5a.25.25 0 1 1 .5 0v2.5a.75.75 0 0 0 1.5 0V6.5a3 3 0 0 0-3-3H7a3 3 0 0 0-3 3v2.75a.75.75 0 0 0 1.5 0v-2.5a.25.25 0 0 1 .5 0"/>
                </svg> Fisioterapia
            </a>
            <div class="collapse" id="longoFisio">
                <asp:TextBox ID="txtLongoFisio" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de fisioterapia (longo prazo)..."></asp:TextBox>
            </div>

            <!-- Nutricionista -->
            <a class="btn btn-outline-danger w-100 text-start mb-2"
                data-bs-toggle="collapse" href="#longoNutri">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-fork-knife" viewBox="0 0 16 16">
                    <path d="M13 .5c0-.276-.226-.506-.498-.465-1.703.257-2.94 2.012-3 8.462a.5.5 0 0 0 .498.5c.56.01 1 .13 1 1.003v5.5a.5.5 0 0 0 .5.5h1a.5.5 0 0 0 .5-.5zM4.25 0a.25.25 0 0 1 .25.25v5.122a.128.128 0 0 0 .256.006l.233-5.14A.25.25 0 0 1 5.24 0h.522a.25.25 0 0 1 .25.238l.233 5.14a.128.128 0 0 0 .256-.006V.25A.25.25 0 0 1 6.75 0h.29a.5.5 0 0 1 .498.458l.423 5.07a1.69 1.69 0 0 1-1.059 1.711l-.053.022a.92.92 0 0 0-.58.884L6.47 15a.971.971 0 1 1-1.942 0l.202-6.855a.92.92 0 0 0-.58-.884l-.053-.022a1.69 1.69 0 0 1-1.059-1.712L3.462.458A.5.5 0 0 1 3.96 0z"/>
                </svg> Nutricionista
            </a>
            <div class="collapse" id="longoNutri">
                <asp:TextBox ID="txtLongoNutri" runat="server" CssClass="form-control mb-2" 
                             TextMode="MultiLine" Rows="3" placeholder="Digite as ações de nutrição (longo prazo)..."></asp:TextBox>
            </div>

     
    </div>
            </div>
</div>

            <br />

<!-- Botões lado a lado -->
<div class="d-flex gap-2" style="width: 815px;">
    
    <asp:Button ID="Button1" runat="server" Text="Salvar" 
                CssClass="btn btn-success flex-fill" OnClick="btnSalvar_Click"
                style="height: 50px; width: fit-content; font-size: x-large; font-weight: 700" />
    
    <asp:Button ID="btnGerarPDF" runat="server" Text="Gerar PDF" 
                CssClass="btn btn-primary flex-fill" OnClick="btnGerarPDF_Click"
                style="width:fit-content; height: 50px; font-size: large; font-weight: 700" />
</div>




    </form>

    <script type="text/javascript">
        // Previne conflitos do Bootstrap com ASP.NET
        var forms = document.getElementsByTagName('form');
        for (var i = 0; i < forms.length; i++) {
            forms[i].addEventListener('submit', function (e) {
                // Expande todos os collapses
                var collapses = document.querySelectorAll('.collapse');
                collapses.forEach(function (c) {
                    c.classList.add('show');
                });
            });
        }
    </script>

</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PTA.aspx.cs" Inherits="ProjetoIntegrador.PTA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PTA</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>



</head>
<body>
                

    <form id="form2" runat="server">
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
                <asp:Button ID="btnSair" CssClass="btn btn-outline-danger" Text="Voltar" OnClick="btnSair_Click" runat="server" />
            </div>
        </div>
    </div>
</nav>
        
        <div class="card" style="margin-top: 70px;">
            <div class="card-body">

        <asp:Button ID="btnEnfermagem" runat="server" class="btn btn-primary" Text="Enfermagem" OnClick="btnEnfermagem_Click" />
        <asp:Button ID="btnFisioterapia"  class="btn btn-primary" Text="Fisioterapia" runat="server" OnClick="btnFisioterapia_Click" />
        <asp:Button ID="btnNutricao"  class="btn btn-primary" Text="Nutrição" runat="server" OnClick="btnNutricao_Click" />
        <asp:Button ID="btnPsicologia"  class="btn btn-primary" Text="Psicologia" runat="server" OnClick="btnPsicologia_Click" />
        <asp:Button ID="btnSocial"  class="btn btn-primary" Text="Serviço Social" runat="server" OnClick="btnSocial_Click" />
                </div>
            </div>
    </form>




    <style>
        .btn {
            margin-right: 5px;
            
            margin-left:5px;
        }

        .card {
            position: absolute;
            left: 50%;
            top: 60px;
            transform: translate(-50%);
            border-color: deepskyblue;
            border-style: ridge;
            padding: 5px;
            border-width: medium;
            border-radius: 20px;
        }
    </style>
</body>
</html>

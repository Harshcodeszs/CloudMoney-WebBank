<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="USER Login.aspx.cs" Inherits="Final_Project1.LOG_IN" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Login</title>
    <link href="CSS/Log in.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="Javascript/script.js" type="text/javascript"></script>
         <script src="https://cdn.botpress.cloud/webchat/v3.6/inject.js"></script>
<script src="https://files.bpcontent.cloud/2026/05/18/05/20260518054555-YJLG6VBW.js" defer="defer"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card">
            <div class="brand">
                <span class="brand-dot"></span>
                <span class="brand-name">CloudMoney</span>
            </div>

            <h1 class="title">Log In</h1>
            <p class="subtitle">Access your digital wallet securely.</p>

            <!-- Username -->
            <div class="field">
                <label class="label">USERNAME</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input" ></asp:TextBox> 
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                    ControlToValidate="txtUsername" ErrorMessage="Required field" 
                    CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <!-- Password -->
            <div class="field">
                <label class="label">PASSWORD</label>
                <div style="position: relative;">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input"   placeholder="••••••••"></asp:TextBox>
                   <!-- eye animation javascipt -->
                    <i class="fa-solid fa-eye" id="toggleEye" 
                       style="position: absolute; right: 12px; top: 11px; cursor: pointer; color: #6B7280; font-size: 14px;"
                       onclick="toggleVisibility('<%= txtPassword.ClientID %>', this)"></i>
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" ErrorMessage="Required field" 
                    CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <asp:HyperLink ID="lnkChange" runat="server" NavigateUrl="Forgot Password.aspx" CssClass="forgot">
                Forgot Password?
            </asp:HyperLink>

            <asp:Button ID="Button1" runat="server" OnClick="LogIn_Click" Text="Log In" CssClass="btn" />

            <div class="divider"></div>

            <div class="footer">
                No Account? 
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="USER REGISTRATION.aspx">Register</asp:HyperLink>
            </div>

            <asp:Label ID="Label1" runat="server" CssClass="msg"></asp:Label>
        </div>
    </form>
</body>
</html>
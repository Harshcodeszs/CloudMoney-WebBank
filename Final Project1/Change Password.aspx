<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change Password.aspx.cs" Inherits="Final_Project1.Change_Password" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Change Password</title>
    <link href="CSS/Change  Password.css" rel="stylesheet" type="text/css" />
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

            <h1 class="title">Security Update</h1>
            <p class="subtitle">Update your account credentials below.</p>

           

            <div class="field">
                <label class="label">CURRENT PASSWORD</label>
                <div style="position: relative;">
                    <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password" CssClass="input" ></asp:TextBox>
                    <i class="fa-solid fa-eye" style="position: absolute; right: 12px; top: 11px; cursor: pointer; color: #6B7280; font-size: 14px;"
                       onclick="toggleVisibility('<%= txtOldPass.ClientID %>', this)"></i>
                </div>
            </div>

            <div class="divider"></div>

            <div class="field">
                <label class="label">NEW PASSWORD</label>
                <div style="position: relative;">
                    <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" CssClass="input" placeholder="New password"></asp:TextBox>
                    <i class="fa-solid fa-eye" style="position: absolute; right: 12px; top: 11px; cursor: pointer; color: #6B7280; font-size: 14px;"
                       onclick="toggleVisibility('<%= txtNewPass.ClientID %>', this)"></i>
                </div>
            </div>

            <div class="field">
                <label class="label">CONFIRM NEW PASSWORD</label>
                <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" CssClass="input" placeholder="Repeat new password"></asp:TextBox>
            </div>

            <asp:Button ID="btnUpdate" runat="server" Text="Update Password" 
                OnClick="btnUpdate_Click" CssClass="btn" />

            <div class="footer" style="margin-top: 1.5rem;">
                <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Dashboard.aspx">Back to Dashboard</asp:HyperLink>
            </div>

            <asp:Label ID="Label1" runat="server" CssClass="msg"></asp:Label>
        </div>
    </form>
</body>
</html>
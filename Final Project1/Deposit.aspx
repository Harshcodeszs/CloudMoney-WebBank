<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Deposit.aspx.cs" Inherits="Final_Project1.Deposit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Deposit</title>
    <script src="Javascript/script.js" type="text/javascript"></script>
      <link href="CSS/Deposit.css" rel="stylesheet" type="text/css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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

        <h1 class="title">Deposit Funds</h1>
        <p class="subtitle">Add money to your digital wallet instantly.</p>

        <div class="balance-box">
            <div class="balance-label">Current Balance</div>
            <div class="balance-value">
                ₱ <asp:Label ID="lblCurrentBalance" runat="server" Text="0.00"></asp:Label>
                <span>PHP</span>
            </div>
        </div>

        <div class="field">
            <label class="label">DEPOSIT AMOUNT</label>
            <asp:TextBox ID="txtAmount" runat="server" CssClass="input" placeholder="0.00"></asp:TextBox>
            <p class="hint">Accepted range: ₱100.00 — ₱2,000.00</p>
            <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount"
                ErrorMessage="Please enter an amount." CssClass="error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnDeposit" runat="server" Text="Confirm Deposit" 
            OnClick="btnDeposit_Click" CssClass="btn" />

        <div class="divider"></div>

        <div class="footer">
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Dashboard.aspx">Back to Dashboard</asp:HyperLink>
        </div>

        <asp:Label ID="lblStatus" runat="server" CssClass="msg"></asp:Label>
    </div>
</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMoney.aspx.cs" Inherits="Final_Project1.SendMoney" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Send Money</title>
       <link href="CSS/Send Money.css" rel="stylesheet" type="text/css" />
     <script src="Javascript/script.js" type="text/javascript"></script>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />


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

        <h1 class="title">Send Money</h1>
        <p class="subtitle">Transfer funds to another CloudMoney user.</p>

        <asp:Panel ID="pnlSearch" runat="server" CssClass="field">
            <label class="label">RECIPIENT ACCOUNT NO</label>
            <div class="input-row">
                <asp:TextBox ID="txtRecipientAcc" runat="server" CssClass="input" placeholder="ex. 10001"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Find User" OnClick="btnSearch_Click" CssClass="btn-find" />
            </div>
            <p class="hint">Ask the recipient for their unique Account Number.</p>
        </asp:Panel>

        <asp:Panel ID="pnlTransfer" runat="server" Visible="false">
            <div class="recipient-box">
                <div class="recipient-tag">Recipient Found</div>
                <div class="recipient-name">
                    <asp:Label ID="lblTargetName" runat="server"></asp:Label>
                </div>
                <div class="recipient-acc">
                    ACC: <asp:Label ID="lblTargetAcc" runat="server"></asp:Label>
                </div>
            </div>

            <div class="field">
                <label class="label">AMOUNT TO SEND</label>
                <asp:TextBox ID="txtAmount" runat="server" CssClass="input" placeholder="0.00"></asp:TextBox>
                <p class="hint">Limit: ₱100.00 — ₱2,000.00</p>
            </div>

           <div class="field">
    <label class="label">CONFIRM YOUR PASSWORD</label>
    <div style="position: relative;">
        <asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password" 
            CssClass="input" placeholder="••••••••"></asp:TextBox>
        
        <i class="fa-solid fa-eye" 
           style="position: absolute; right: 12px; top: 11px; cursor: pointer; color: #6B7280; font-size: 14px;"
           onclick="toggleVisibility('<%= txtConfirmPass.ClientID %>', this)"></i>
    </div>
</div>

            <asp:Button ID="btnSend" runat="server" Text="Confirm Transfer" OnClick="btnSend_Click" CssClass="btn" />
        </asp:Panel>

        <div class="divider"></div>

        <div class="footer">
            <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Dashboard.aspx">Back to Dashboard</asp:HyperLink>
        </div>

        <asp:Label ID="lblStatus" runat="server" CssClass="msg"></asp:Label>
    </div>
</form>
</body>
</html>

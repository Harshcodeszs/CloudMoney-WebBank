<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgot Password.aspx.cs" Inherits="Final_Project1.Forgot_Password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <link href="CSS/Forget Password.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
         <script src="https://cdn.botpress.cloud/webchat/v3.6/inject.js"></script>
     <script src="Javascript/script.js" type="text/javascript"></script>
<script src="https://files.bpcontent.cloud/2026/05/18/05/20260518054555-YJLG6VBW.js" defer="defer"></script>
</head>
<body>
   <form id="form1" runat="server">
    <div class="card">
        <div class="brand">
            <span class="brand-dot"></span>
            <span class="brand-name">CloudMoney</span>
        </div>

        <div class="steps">
            <div class="step-dot active">1</div>
            <div class="step-line"></div>
            <div class="step-dot inactive">2</div>
        </div>

        <asp:Panel ID="pnlSearch" runat="server">
            <div class="title">Find your account</div>
            <div class="field">
                <label class="label">Username</label>
                <asp:TextBox ID="txtSearchUser" runat="server" CssClass="input" ValidationGroup="Step1" />
                <asp:RequiredFieldValidator ID="rfvSearch" runat="server" 
                    ControlToValidate="txtSearchUser" ValidationGroup="Step1"
                    ErrorMessage="Username is required." CssClass="error" Display="Dynamic" ForeColor="Red" />
            </div>
            <asp:Button ID="btnFind" runat="server" Text="Find Account" 
                OnClick="btnFind_Click" ValidationGroup="Step1" CssClass="btn" />
        </asp:Panel>

        <asp:Panel ID="pnlVerify" runat="server" Visible="false">
            <div class="title">Reset password</div>
            
            <div class="field">
                <label class="label">6-Digit Backup Code</label>
                <asp:TextBox ID="txtBackupVerify" runat="server" CssClass="code-input" MaxLength="6" ValidationGroup="Step2" placeholder="······" />
                <asp:RequiredFieldValidator ID="rfvBackup" runat="server" 
                    ControlToValidate="txtBackupVerify" ValidationGroup="Step2"
                    ErrorMessage="Required" CssClass="error" Display="Dynamic" ForeColor="Red" />
                <asp:RegularExpressionValidator ID="revBackup" runat="server" 
                    ControlToValidate="txtBackupVerify" ValidationGroup="Step2"
                    ValidationExpression="^\d{6}$" ErrorMessage="Must be 6 digits" CssClass="error" Display="Dynamic" ForeColor="Red" />
            </div>

            <div class="field">
    <label class="label">New Password</label>
    
    <div style="position: relative; display: block;">
        <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" 
            CssClass="input" ValidationGroup="Step2" placeholder="Enter new password" />
        
        <i class="fa-solid fa-eye" 
           style="position: absolute; right: 12px; top: 50%; transform: translateY(-50%); cursor: pointer; color: #6B7280; font-size: 16px; z-index: 999;" 
           onclick="toggleVisibility('<%= txtNewPass.ClientID %>', this)"></i>
    </div>

    <asp:RequiredFieldValidator ID="rfvNew" runat="server" 
        ControlToValidate="txtNewPass" ValidationGroup="Step2"
        ErrorMessage="Required" CssClass="error" Display="Dynamic" ForeColor="Red" />
</div>
               
           

            <div class="field">
                <label class="label">Confirm Password</label>
                <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" CssClass="input" ValidationGroup="Step2" />
                <asp:CompareValidator ID="cvConfirm" runat="server" 
                    ControlToValidate="txtConfirm" ControlToCompare="txtNewPass" 
                    ValidationGroup="Step2" ErrorMessage="Mismatch" CssClass="error" Display="Dynamic" ForeColor="Red" />
            </div>
       
            <asp:Button ID="btnReset" runat="server" Text="Reset Password" 
                OnClick="btnReset_Click" ValidationGroup="Step2" CssClass="btn" />
        </asp:Panel>

        <div class="divider"></div>
        <div class="footer">
            Remembered it? <a href="USER Login.aspx">Back to Log in</a>
        </div>
    </div>
</form>
    


</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="USER REGISTRATION.aspx.cs" Inherits="Final_Project1.REGISTRATION" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Register</title>
    <link href="CSS/Register.css" rel="stylesheet" type="text/css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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
            <h1 class="title">Create Account</h1>
            <p class="subtitle">Fill in the details below to get started.</p>

            <div class="field-row">
            <!-- First Name -->
            <div class="field">
                <label class="label" for="txtFname">First Name</label>
                <asp:TextBox ID="txtFname" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvFname" runat="server"
                    ControlToValidate="txtFname"
                    ErrorMessage="First name is required."
                    CssClass="error" ForeColor="" Display="Dynamic" />
            </div>

            <!-- Last Name -->
            <div class="field">
                <label class="label" for="txtLname">Last Name</label>
                <asp:TextBox ID="txtLname" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvLname" runat="server"
                    ControlToValidate="txtLname"
                    ErrorMessage="Last name is required."
                    CssClass="error" ForeColor="" Display="Dynamic" />
            </div>
                </div>

            <!-- Username -->
            <div class="field">
                <label class="label" for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                    ControlToValidate="txtUsername"
                    ErrorMessage="Username is required."
                    CssClass="error" ForeColor="" Display="Dynamic" />
            </div>

             <!-- Backup Code -->
            <div class="field">
             <label class="label">6-DIGIT RECOVERY PIN</label>
             <asp:TextBox ID="txtBackup" runat="server" CssClass="input" placeholder="6 digits pin" MaxLength="6"></asp:TextBox>
    
                <asp:RegularExpressionValidator ID="revBackup" runat="server" 
              ControlToValidate="txtBackup" ValidationExpression="^\d{6}$" 
                 ErrorMessage="Must be exactly 6 digits." CssClass="error" Display="Dynamic" ForeColor="Red">
                </asp:RegularExpressionValidator>
    
             <asp:RequiredFieldValidator ID="rfvBackup" runat="server" 
        ControlToValidate="txtBackup" ErrorMessage="Recovery PIN is required." 
        CssClass="error" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>

            <!-- Password -->
            <div class="field">
                <label class="label" for="txtPassword">Password</label>
                <div class="password-wrapper">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input"  placeholder="••••••••" />
                       <!-- eye animation javascipt -->
                  <i class="fa-solid fa-eye toggle-password" onclick="toggleVisibility('<%= txtPassword.ClientID %>', this)"></i>
                    </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ErrorMessage="Password is required."
                    CssClass="error" ForeColor="" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                    ErrorMessage="Use 8+ characters with letters and numbers."
                    CssClass="error" ForeColor="" Display="Dynamic" />
            </div>

            <!-- Confirm Password -->
            <div class="field">
                <label class="label" for="txtCpassword">Confirm Password</label>
                <div class="password-wrapper">
                     <!-- eye animation javascipt -->
                <asp:TextBox ID="txtCpassword" runat="server" TextMode="Password" CssClass="input"  placeholder="••••••••" />
                    <i class="fa-solid fa-eye toggle-password" onclick="toggleVisibility('<%= txtCpassword.ClientID %>', this)"></i>
                    </div>
                <asp:CompareValidator ID="cvCpassword" runat="server"
                    ControlToValidate="txtCpassword"
                    ControlToCompare="txtPassword"
                    ErrorMessage="Passwords don't match."
                    CssClass="error" ForeColor="" Display="Dynamic" />
            </div>

            <!-- Submit -->
            <asp:Button ID="Button1" runat="server"
                Text="Sign Up"
                CssClass="btn"
                OnClick="Register_Click" />

            <!-- Footer -->
            <div class="footer">
                Already have an account?
                <asp:HyperLink ID="lnkChange" runat="server" NavigateUrl="USER Login.aspx">Log in</asp:HyperLink>
            </div>
        </div>

    </form>
</body>
</html>

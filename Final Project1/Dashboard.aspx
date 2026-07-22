<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Final_Project1.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Dashboard</title>
    <link href="CSS/Dashboard.css" rel="stylesheet" type="text/css" />
    
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
    <style>
         /* total send */

.stats-row {
    display: grid !important;
    grid-template-columns: 1fr 1fr !important;
    gap: 1rem !important;
}
.balance-value, .totalsent-value {
    font-size: 28px !important;
    font-weight: 700 !important;
    color: #0D1B3E !important;
    letter-spacing: -0.5px !important;
    margin: 0 !important;
}
.balance-label, .totalsent-label {
    font-size: 11px !important;
    font-weight: 500 !important;
    color: #6B7280 !important;
    letter-spacing: 0.06em !important;
    text-transform: uppercase !important;
    margin-bottom: 6px !important;
    display: block !important;
}
.balance-value span, .totalsent-value span {
    font-size: 14px !important;
    font-weight: 400 !important;
    color: #6B7280 !important;
}


    /* recieved */

    .activity-card {
        background: #ffffff !important;
        border: 0.5px solid rgba(0, 0, 0, 0.1) !important;
        border-radius: 12px !important;
        padding: 1.25rem !important;
        margin-bottom: 1.25rem !important;
    }

    .activity-item {
        display: flex !important;
        align-items: center !important;
        gap: 12px !important;
        padding: 0.65rem 0 !important;
    }

    .activity-icon {
        width: 34px !important;
        height: 34px !important;
        min-width: 34px !important;
        border-radius: 50% !important;
        background: #DCFCE7 !important;
        display: flex !important;
        align-items: center !important;
        justify-content: center !important;
    }

    .activity-info {
        flex: 1 !important;
    }

    .activity-name {
        font-size: 13px !important;
        font-weight: 500 !important;
        color: #0D1B3E !important;
    }

    .activity-sub {
        font-size: 11px !important;
        color: #9CA3AF !important;
    }

    .activity-amount-pos {
        font-size: 13px !important;
        font-weight: 600 !important;
        color: #16A34A !important;
        white-space: nowrap !important;
    }



    
</style>
    
     <script src="https://cdn.botpress.cloud/webchat/v3.6/inject.js"></script>
<script src="https://files.bpcontent.cloud/2026/05/18/05/20260518054555-YJLG6VBW.js" defer="defer"></script>
    
</head>
<body>
   <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        
    <div class="page">
       
           <div class="topbar">
    <div class="brand">
        <span class="brand-dot"></span>
        <span class="brand-name">CloudMoney</span>
    </div>
    <div style="display:flex; align-items:center; gap:8px; flex-shrink:0;">
        <a href="Change Password.aspx" style="height:34px; padding:0 1rem; background:transparent; color:#6B7280; border:0.5px solid rgba(0,0,0,0.15); border-radius:8px; font-size:12px; font-weight:500; text-decoration:none; display:inline-flex; align-items:center; white-space:nowrap;">Change Password</a>
        <asp:Button ID="btnLogout" runat="server" Text="Sign Out" OnClick="btnLogout_Click" CssClass="btn-logout" />
    </div>
</div>

        <div class="profile-card">
            <div class="profile-top">
                <div>
                    <div class="profile-name"><asp:Label ID="lblFullName" runat="server"></asp:Label></div>
                    <p class="profile-acc">Acc No: <asp:Label ID="lblAccNo" runat="server"></asp:Label></p>
                    <p class="profile-date">Member since <asp:Label ID="lblRegDate" runat="server"></asp:Label></p>

              
                </div>
                <div class="avatar">
                    <asp:Literal ID="litInitials" runat="server"></asp:Literal>
                </div>
            </div>

            <div class="divider"></div>   

           <div class="stats-row">
    <div class="balance-section">
        <p class="balance-label">Current Balance</p>
<div class="balance-value">₱ <asp:Label ID="lblBalance" runat="server" Text="0.00"></asp:Label> <span>PHP</span></div>
    </div>
    <div class="totalsent-section">
        <p class="totalsent-label">Total Sent</p>
<div class="totalsent-value">₱ <asp:Label ID="lblTotalSent" runat="server" Text="0.00"></asp:Label> <span>PHP</span></div>
    </div>
</div>
        </div>

  <div class="activity-card">
    <span class="activity-title">Recently Recieved CloudMoney</span>
    <asp:PlaceHolder ID="phNotify" runat="server" Visible="false">
        <div class="activity-item">
            <div class="activity-icon" style="background:#DCFCE7">
                <i class="fa-solid fa-arrow-down" style="color:#16A34A;font-size:12px"></i>
            </div>
            <div class="activity-info">
                <div class="activity-name">Received from Acc #<asp:Label ID="lblNotifySender" runat="server" /></div>
                <div class="activity-sub">
               <asp:Label ID="lblActivityDate" runat="server" Text="today" />
                    </div>
            </div>
            <span class="activity-amount-pos">+ ₱<asp:Label ID="lblNotifyAmount" runat="server" /></span>
        </div>
    </asp:PlaceHolder>
</div>

        <p class="section-label">Quick Actions</p>

        <div class="actions-grid">
            <a href="Deposit.aspx" class="action-card">
                <div class="action-icon"><i class="fa-solid fa-plus" style="color: #16A34A;"></i></div>
                <div>
                    <div class="action-title">Deposit</div>
                    <div class="action-desc">Add funds to wallet</div>
                </div>
            </a>

            <a href="Withdraw.aspx" class="action-card">
                <div class="action-icon"><i class="fa-solid fa-arrow-up-from-bracket" style="color: #DC2626;"></i></div>
                <div>
                    <div class="action-title">Withdraw</div>
                    <div class="action-desc">Cash out your funds</div>
                </div>
            </a>

            <a href="SendMoney.aspx" class="action-card">
                <div class="action-icon"><i class="fa-solid fa-paper-plane" style="color: #0D1B3E;"></i></div>
                <div>
                    <div class="action-title">Send Money</div>
                    <div class="action-desc">Transfer to others</div>
                </div>
            </a>

            <a href="Reports.aspx" class="action-card">
                <div class="action-icon"><i class="fa-solid fa-chart-line" style="color: #C9A84C;"></i></div>
                <div>
                    <div class="action-title">Reports</div>
                    <div class="action-desc">View history</div>
                </div>
                 
            </a>
        </div> 
    </div> 

</form>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            console.log("Security Kernel Loaded and DOM Ready");
        });
    </script> 

    <script src="Javascript/script.js" type="text/javascript"></script>
  
</body>
</html>
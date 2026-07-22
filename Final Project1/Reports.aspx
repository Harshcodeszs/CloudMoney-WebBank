<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="Final_Project1.Reports" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <title>Reports</title>
    
    <link href="CSS/Reports.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />

       <script src="https://cdn.botpress.cloud/webchat/v3.6/inject.js"></script>
<script src="https://files.bpcontent.cloud/2026/05/18/05/20260518054555-YJLG6VBW.js" defer="defer"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page">
            <div class="brand">
                <span class="brand-dot"></span>
                <span class="brand-name">CloudMoney</span>
            </div>
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
            <h1 class="title">Transaction Reports</h1>
            <p class="subtitle">Review your financial activity and account history.</p>

            <div class="filter-card">
                <div class="filter-row">
                    <div class="field">
                        <label class="label">FROM DATE</label>
                        <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="input"></asp:TextBox>
                    </div>
                    <div class="field">
                        <label class="label">TO DATE</label>
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="input"></asp:TextBox>
                    </div>
                    <div class="field">
                        <label class="label">CATEGORY</label>
                        <asp:DropDownList ID="ddlReportCategory" runat="server" CssClass="input" AutoPostBack="true" 
                            OnSelectedIndexChanged="ddlReportCategory_SelectedIndexChanged">
                            <asp:ListItem Value="Statement">Statement of Account</asp:ListItem>
                            <asp:ListItem Value="Cash">Deposits & Withdrawals</asp:ListItem>
                            <asp:ListItem Value="Transfer">Sent & Received</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button ID="btnList" runat="server" Text="Generate List" OnClick="btnList_Click" CssClass="btn-list" />
                    <asp:HyperLink ID="lnkDash" runat="server" NavigateUrl="Dashboard.aspx" CssClass="btn-dash">
                        <i class="fa-solid fa-house" style="margin-right: 8px;"></i> Dashboard
                    </asp:HyperLink>
                </div>
            </div>

         <div class="table-card">
    <div class="table-header">
        <span class="table-title">Activity Log</span>
        <span class="table-count">Displaying latest transactions</span>
    </div>

    <div style="overflow-x: auto; -webkit-overflow-scrolling: touch; width: 100%;">
    <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="False" CssClass="table" GridLines="None" ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:BoundField DataField="TransDate" HeaderText="DATE" DataFormatString="{0:MMM dd, yyyy<br/>hh:mm tt}" HtmlEncode="false" />
                
                
                <asp:TemplateField HeaderText="TRANSACTION ID" ItemStyle-HorizontalAlign="Right">
    <ItemTemplate>
       <span class="transaction-id-style">
            #<%# Eval("TransID") %></span>
    </ItemTemplate>
</asp:TemplateField>

                <asp:TemplateField HeaderText="TYPE">
                    <ItemTemplate>
                        <span class='<%# "type-badge type-" + Eval("TransType").ToString().ToLower() %>'>
                            <%# Eval("TransType") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="FROM">
                    <ItemTemplate>
                        <strong><%# Eval("TransType").ToString() == "Deposit" ? "—" : Eval("FromAcc") %></strong><br />
                        <small><%# Eval("TransType").ToString() == "Deposit" ? "External" : Eval("FromName") %></small>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="TO">
                    <ItemTemplate>
                        <strong><%# Eval("TransType").ToString() == "Withdraw" ? "—" : Eval("ToAcc") %></strong><br />
                        <small><%# Eval("TransType").ToString() == "Withdraw" ? "External" : Eval("ToName") %></small>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="AMOUNT" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <span class='<%# Eval("TransType").ToString() == "Withdrawal" || Eval("TransType").ToString() == "Sent" ? "amount-neg" : "amount-pos" %>'>
                            <%# (Eval("TransType").ToString() == "Withdrawal" || Eval("TransType").ToString() == "Sent" ? "- " : "+ ") %>
                            ₱<%# Eval("Amount", "{0:N2}") %></span>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <EmptyDataTemplate>
                <div style="padding: 2rem; text-align: center; color: #6B7280; font-size: 13px;">
                    No transactions found for the selected period.
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</div>

</div>
            
       
    </form>
</body>
</html>

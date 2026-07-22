using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Final_Project1
{
    public partial class Deposit : System.Web.UI.Page
    {
        //connection string
        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            //  If they aren't logged in, send them back
            if (Session["AccNo"] == null)
            {
                Response.Redirect("USER LOGIN.aspx");
            }

            if (!IsPostBack) // Only runs the first time the page loads
            {

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "SELECT ISNULL(Balance, 0) FROM Useraccount WHERE AccountNo = @acc";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@acc", Session["AccNo"].ToString());

                    conn.Open();
                    //asking for one single value which is balance reader
                    lblCurrentBalance.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N2");
                }
            }
        }

        private void RunSweetAlert(string title, string text, string icon)
        {
            string script = $@"Swal.fire({{
            title: '{title}',
            text: '{text}',
            icon: '{icon}',
            confirmButtonColor: '#4A90E2'
        }});";
            ClientScript.RegisterStartupScript(this.GetType(), "sweetalert", script, true);
        }

        protected void btnDeposit_Click(object sender, EventArgs e)
        {
            decimal depositAmount;
            // 1.  Is it a number?
            if (!decimal.TryParse(txtAmount.Text, out depositAmount))
            {
                RunSweetAlert("Invalid Input", "Please enter a valid numeric amount.", "error");
                return;
            }

            // Minimum 100 and Maximum 2,000
            if (depositAmount < 100 || depositAmount > 2000)
            {
                RunSweetAlert("Limit Check", "Deposit must be between 100.00 and 2,000.00.", "warning");
                return;
            }

            // Must be divisible by 100
            if (depositAmount % 100 != 0)
            {
                RunSweetAlert("Incorrect Format", "Amount must be in multiples of 100.00.", "info");
                return;
            }

            //live balance sa database
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 2. Fetch current balance to check the 10,000 limit
                string checkSql = "SELECT ISNULL(Balance, 0) FROM Useraccount WHERE AccountNo = @acc";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@acc", Session["AccNo"].ToString());

                //retrieving a specific number from database and preparing it for use in calculations.
                decimal currentBalance = Convert.ToDecimal(checkCmd.ExecuteScalar());

                //  Total balance must not exceed 10,000
                if ((currentBalance + depositAmount) > 10000)
                {
                    RunSweetAlert("Wallet Full", "Transaction failed. Total balance cannot exceed 10,000.00.", "error"); return;
                }

                // 3. Update the Balance
                string updateSql = "UPDATE Useraccount SET Balance = Balance + @amt WHERE AccountNo = @acc";
                using (SqlCommand updateCmd = new SqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("@amt", depositAmount);
                    updateCmd.Parameters.AddWithValue("@acc", Session["AccNo"].ToString());
                    
                    //perform
                    updateCmd.ExecuteNonQuery();

                    RunSweetAlert("Success!", "Deposit of " + depositAmount.ToString("N2") + " processed.", "success");

                    //connect database transactions
                    //data sa transactions 
                    string logSql = "INSERT INTO Transactions (AccountNo, TransType, Amount) VALUES (@acc, 'Deposit', @amt)";
                    SqlCommand logCmd = new SqlCommand(logSql, conn);
                    logCmd.Parameters.AddWithValue("@acc", Session["AccNo"]);
                    logCmd.Parameters.AddWithValue("@amt", txtAmount.Text);
                    logCmd.ExecuteNonQuery();

                    // Update the display label
                    lblCurrentBalance.Text = (currentBalance + depositAmount).ToString("N2");
                    txtAmount.Text = ""; 
                }
            }
        }
    }
}
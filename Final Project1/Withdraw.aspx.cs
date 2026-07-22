using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Final_Project1
{
    public partial class Withdraw : System.Web.UI.Page
    {

        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            //  If they aren't logged in, send them back
            if (Session["AccNo"] == null)
            {
                Response.Redirect("USER LOGIN.aspx");
            }
            else if (!IsPostBack) // Only runs the first time the page loads
            {
                DisplayBalance(); 
            }
        }

        private void DisplayBalance()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Requirement: Show balance before performing transaction
                string sql = "SELECT ISNULL(Balance, 0) FROM Useraccount WHERE AccountNo = @acc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@acc", Session["AccNo"].ToString());
                conn.Open();
                lblCurrentBalance.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N2");
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

        protected void btnWithdraw_Click(object sender, EventArgs e)
        {
            decimal withdrawAmount;

            // 1. Validate Input (Numeric, 100-2000, divisible by 100)
            if (!decimal.TryParse(txtAmount.Text, out withdrawAmount))
            {
                RunSweetAlert("Invalid Input", "Please enter a valid numeric amount.", "error"); return;
            }
            if (withdrawAmount < 100 || withdrawAmount > 2000 || withdrawAmount % 100 != 0)
            {
                RunSweetAlert("Requirement Unmet", "Withdrawal must be 100–2,000 and a multiple of 100.", "warning");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 2. Fetch current balance to check for "Insufficient Funds"
                string checkSql = "SELECT ISNULL(Balance, 0) FROM Useraccount WHERE AccountNo = @acc";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@acc", Session["AccNo"].ToString());
                decimal currentBalance = Convert.ToDecimal(checkCmd.ExecuteScalar());

                if (withdrawAmount > currentBalance)
                {
                    RunSweetAlert("Insufficient Funds", "You only have ₱" + currentBalance.ToString("N2"), "error"); lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // 3. Perform the Withdrawal
                string updateSql = "UPDATE Useraccount SET Balance = Balance - @amt WHERE AccountNo = @acc";
                using (SqlCommand updateCmd = new SqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("@amt", withdrawAmount);
                    updateCmd.Parameters.AddWithValue("@acc", Session["AccNo"].ToString());

                    updateCmd.ExecuteNonQuery();

                    //connect database transactions
                    //data sa transactions 
                    string logSql = "INSERT INTO Transactions (AccountNo, TransType, Amount) VALUES (@acc, 'Withdrawal', @amt)";
                    SqlCommand logCmd = new SqlCommand(logSql, conn);
                    logCmd.Parameters.AddWithValue("@acc", Session["AccNo"].ToString());
                    logCmd.Parameters.AddWithValue("@amt", withdrawAmount); // Make sure this is the decimal variable
                    logCmd.ExecuteNonQuery();

                    
                    RunSweetAlert("Success!", "You have withdrawn ₱" + withdrawAmount.ToString("N2"), "success");

                    txtAmount.Text = ""; 
                    DisplayBalance(); // Update 
                }


            }
        }
    }
}
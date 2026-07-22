using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Final_Project1
{
    public partial class SendMoney : System.Web.UI.Page
    {

        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Security check: If they aren't logged in, send them back
            if (Session["AccNo"] == null)
            {
                Response.Redirect("USER LOGIN.aspx");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int parsedAccount;
            if (!int.TryParse(txtRecipientAcc.Text.Trim(), out parsedAccount))
            {
                lblStatus.Text = "Please enter a valid numeric account number.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                pnlTransfer.Visible = false;
                return; 
            }


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT FirstName, LastName FROM Useraccount WHERE AccountNo = @acc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@acc", txtRecipientAcc.Text.Trim());

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                //acc number
                if (reader.Read())
                {
                    lblTargetAcc.Text = txtRecipientAcc.Text;

                    // Combine the names into one string
                    string fullName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();

                    // Use TextInfo to capitalize the first letters
                    // We use .ToLower() first so that names in ALL CAPS are fixed correctly
                    System.Globalization.TextInfo ti = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
                    lblTargetName.Text = ti.ToTitleCase(fullName.ToLower());

                    pnlTransfer.Visible = true; // Show the transfer fields
                    lblStatus.Text = "";

                    if (lblTargetAcc.Text == Session["AccNo"].ToString())
                    {
                        lblStatus.Text = "You cannot transfer money to your own account.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        pnlTransfer.Visible = false;
                        return; 

                    }
                    
                    pnlTransfer.Visible = true;
                    lblStatus.Text = "";
                }
                else
                {
                    lblStatus.Text = "Recipient account not found.";
                    pnlTransfer.Visible = false;
                }

            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            decimal amount;
            // 1. Validation Rules
            if (!decimal.TryParse(txtAmount.Text, out amount) || amount < 100 || amount > 2000 || amount % 100 != 0)
            {
                lblStatus.Text = "Amount must be 100-2000 and a multiple of 100.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 2. Security Check: Verify Current User's Password & Balance
                
                string securitySql = "SELECT Password, Balance FROM Useraccount WHERE AccountNo = @me";
                SqlCommand securityCmd = new SqlCommand(securitySql, conn);
                securityCmd.Parameters.AddWithValue("@me", Session["AccNo"].ToString());

                SqlDataReader reader = securityCmd.ExecuteReader();
                if (reader.Read())
                {
                    string dbPass = reader["Password"].ToString();
                    decimal myBalance = Convert.ToDecimal(reader["Balance"]);
                    reader.Close(); // Close reader to perform updates

                    if (txtConfirmPass.Text != dbPass)
                    {
                        lblStatus.Text = "Incorrect password verification!";
                        return;
                    }
                    if (amount > myBalance)
                    {
                        lblStatus.Text = "Insufficient funds!";
                        return;
                    }


                  

                    // 3. THE TRANSFER (Atomic Transaction)
                    SqlTransaction trans = conn.BeginTransaction();
                    try
                    {
                        // balance after sending 
                        //balancetest
                        //Subtract from Me.
                        string subSql = "UPDATE Useraccount SET Balance = Balance - @amt WHERE AccountNo = @me";
                        SqlCommand subCmd = new SqlCommand(subSql, conn, trans);
                        subCmd.Parameters.AddWithValue("@amt", amount);
                        subCmd.Parameters.AddWithValue("@me", Session["AccNo"].ToString());
                        subCmd.ExecuteNonQuery();

                        // Add to RECIPIENT
                        //Add to You.
                        string addSql = "UPDATE Useraccount SET Balance = Balance + @amt WHERE AccountNo = @target";
                        SqlCommand addCmd = new SqlCommand(addSql, conn, trans);
                        addCmd.Parameters.AddWithValue("@amt", amount);
                        addCmd.Parameters.AddWithValue("@target", lblTargetAcc.Text);
                        addCmd.ExecuteNonQuery();

                        // 1. Log for the SENDER for my report
                        //subtract money from my acc
                        //Log my "Sent" history.
                        string logSent = "INSERT INTO Transactions (AccountNo, TransType, Amount, RelatedAccount) VALUES (@me, 'Sent', @amt, @target)";
                        SqlCommand cmdSent = new SqlCommand(logSent, conn, trans);
                        cmdSent.Parameters.AddWithValue("@me", Session["AccNo"].ToString());
                        cmdSent.Parameters.AddWithValue("@amt", amount);
                        cmdSent.Parameters.AddWithValue("@target", lblTargetAcc.Text); // Who you sent it to
                        cmdSent.ExecuteNonQuery();

                        // 2. Log for the RECIPIENT for their report
                        //add money sa reciever
                        //Log your "Received" history.
                        string logRec = "INSERT INTO Transactions (AccountNo, TransType, Amount, RelatedAccount) VALUES (@target, 'Received', @amt, @me)";
                        SqlCommand cmdRec = new SqlCommand(logRec, conn, trans);
                        cmdRec.Parameters.AddWithValue("@target", lblTargetAcc.Text);
                        cmdRec.Parameters.AddWithValue("@amt", amount);
                        cmdRec.Parameters.AddWithValue("@me", Session["AccNo"].ToString()); // Who they got it from
                        cmdRec.ExecuteNonQuery();

                        trans.Commit(); //save changes
                        lblStatus.Text = "Transfer Successful!";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                     
                    }
                    catch
                    {
                        trans.Rollback(); //undo
                        lblStatus.Text = "Error during transfer. Try again.";
                    }
                }
            }
        }
    }
    }

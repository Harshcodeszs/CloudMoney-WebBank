using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;


namespace Final_Project1
{
    public partial class Dashboard : System.Web.UI.Page
    {

        //connection string
        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.ContentType = "text/html";

            // 1. Check if the user is actually logged in
            if (Session["AccNo"] != null)
            {
                // Get the Account Number from Session
                string myAcc = Session["AccNo"].ToString();

                // 2. Set the labels from the Session data
                lblAccNo.Text = myAcc;

                if (Session["UserFullName"] != null)
                {
                    string rawName = Session["UserFullName"].ToString().ToLower();
                    lblFullName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rawName);


                    // --- START AVATAR INITIALS LOGIC ---
                    string[] nameParts = rawName.Trim().Split(' ');
                    if (nameParts.Length >= 2)
                    {
                        // Takes first letter of first word and first letter of last word
                        litInitials.Text = (nameParts[0][0].ToString() + nameParts[nameParts.Length - 1][0].ToString()).ToUpper();
                    }
                    else if (nameParts.Length == 1)
                    {
                        // Fallback for single names
                        litInitials.Text = nameParts[0][0].ToString().ToUpper();
                    }

                }


                else
                {
                    lblFullName.Text = "No Name Found";
                }

                lblRegDate.Text = Session["RegDate"] != null ? Session["RegDate"].ToString() : "No Date Found";

                // 3. Fetch everything in ONE database trip
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = @"
        -- Result 1: Live Balance
        SELECT ISNULL(Balance, 0) FROM Useraccount WHERE AccountNo = @acc;
        
        -- Result 2: Total Sent
        SELECT ISNULL(SUM(Amount), 0) FROM Transactions 
        WHERE AccountNo = @acc AND TransType = 'Sent';

        -- Result 3: Recent Transaction with TransDate
        SELECT TOP 1 Amount, RelatedAccount, TransDate 
        FROM Transactions 
        WHERE AccountNo = @acc AND TransType = 'Received' 
        AND TransDate > DATEADD(day, -3, GETDATE())
        ORDER BY TransDate DESC;";



                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@acc", myAcc);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Result 1: Live Balance
                        if (reader.Read())
                        {
                            lblBalance.Text = Convert.ToDecimal(reader[0]).ToString("N2");
                        }

                        // Result 2: Total Sent
                        if (reader.NextResult() && reader.Read())
                        {
                            lblTotalSent.Text = Convert.ToDecimal(reader[0]).ToString("N2");
                        }

                        // Result 3: Notification 
                        if (reader.NextResult() && reader.Read())
                        {
                            decimal amt = Convert.ToDecimal(reader["Amount"]);
                            DateTime transDate = Convert.ToDateTime(reader["TransDate"]);
                            //prevent crashing or missing data
                            string senderAcc = reader["RelatedAccount"] != DBNull.Value
                                               ? reader["RelatedAccount"].ToString()
                                               : "Unknown";



                            // 2. THE NEW LOGIC: Determine if it's Today or Yesterday
                            string dateDisplay = "";
                            if (transDate.Date == DateTime.Today)
                            {
                                dateDisplay = "today";
                            }
                            else if (transDate.Date == DateTime.Today.AddDays(-1))
                            {
                                dateDisplay = "yesterday";
                            }
                            else
                            {
                                dateDisplay = transDate.ToString("MMM dd"); // e.g., "May 17"
                            }

                            // Update 
                            lblNotifyAmount.Text = amt.ToString("N2");
                            lblNotifySender.Text = senderAcc;
                            lblActivityDate.Text = dateDisplay;
                            phNotify.Visible = true;

                            // Trigger SweetAlert popup
                            string noteScript = $@"Swal.fire({{
                        title: 'Money Received!',
                        text: 'You just received ₱{amt:N2} from Account {senderAcc}',
                        icon: 'success', toast: true, position: 'top-end',
                        showConfirmButton: false, timer: 5000
                    }});";
                            ClientScript.RegisterStartupScript(this.GetType(), "receivedNote", noteScript, true);
                        }
                        else
                        {
                            phNotify.Visible = false;
                        }
                    }
                } // Closing the Connection using block correctly
            }
            else
            {
                // 4. If the session is NULL, send them back to login
                Response.Redirect("USER LOGIN.aspx");
            }

          
        }
        

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all sessions and go back to Login
            Session.Abandon();
            Response.Redirect("USER LOGIN.aspx");
        }
    }
}
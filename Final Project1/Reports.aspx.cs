using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization; // Added for TextInfo
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Final_Project1
{
    public partial class Reports : System.Web.UI.Page
    {
        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AccNo"] == null)
            {
                Response.Redirect("USER LOGIN.aspx");
            }

            if (!IsPostBack)
            {
                // Set default dates to today
                txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            LoadReports(); // Just call the master method to keep logic consistent
        }

        protected void ddlReportCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReports(); // Refresh when dropdown changes
        }

        private void LoadReports()
        {
            if (Session["AccNo"] == null) return;
            string myAcc = Session["AccNo"].ToString();

            // Validate dates
            //hold date and time
            DateTime fromDate, toDate;
            if (!DateTime.TryParse(txtFromDate.Text, out fromDate)) return;
            if (!DateTime.TryParse(txtToDate.Text, out toDate)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // We use T.AccountNo and T.RelatedAccount to find the names from Useraccount table
                string sql = @"
                SELECT 
                    t.TransID,
                    t.TransDate, 
                    t.TransType, 
                    t.Amount,
                --where the money started
                --If I received it, the sender is the RelatedAccount
                  CASE 
                     WHEN t.TransType = 'Received' THEN t.RelatedAccount 
                --Otherwise (Deposit or Sent)
                        ELSE t.AccountNo 
                    END AS FromAcc,
                  CASE 
                 --u2 is the 'Other Person', u1 is the 'Logged-in User'
                     WHEN t.TransType = 'Received' THEN (u2.FirstName + ' ' + u2.LastName) 
                         ELSE (u1.FirstName + ' ' + u1.LastName) 
                    END AS FromName,
        
                --where the money is going
                 -- If I sent it, the target is the RelatedAccount
                  CASE 
                 --If the user SENT money, the destination is the 'RelatedAccount'
                     WHEN t.TransType = 'Sent' THEN t.RelatedAccount 
                --Otherwise (Received or Deposit)
                    ELSE t.AccountNo 
                 END AS ToAcc,
                 --picking the correct Name for the 'To'
                    CASE 
                      WHEN t.TransType = 'Sent' THEN (u2.FirstName + ' ' + u2.LastName) 
                     ELSE (u1.FirstName + ' ' + u1.LastName) 
                    END AS ToName
                --data source tables
                FROM Transactions t
                --Join u1 Finds the name of the main account holder
                 INNER JOIN Useraccount u1 ON t.AccountNo = u1.AccountNo
                --Join u2: Finds the name of the 'other person' (RelatedAccount)
                --use LEFT JOIN so the row still shows up even if the other person is deleted
                    LEFT JOIN Useraccount u2 ON t.RelatedAccount = u2.AccountNo
                --Only show rows belonging to the person currently logged in
                 WHERE t.AccountNo = @acc 
                --Only show transactions within the dates selected in the UI
                     AND t.TransDate BETWEEN @from AND @to ";

                // Filter by Category
                string category = ddlReportCategory.SelectedValue;
                if (category == "Cash")
                {
                    sql += " AND t.TransType IN ('Deposit', 'Withdrawal') ";
                }
                else if (category == "Transfer")
                {
                    sql += " AND t.TransType IN ('Sent', 'Received') ";
                }
                // 6. Show the newest transactions at the very top
                sql += " ORDER BY t.TransDate DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@acc", myAcc);
                cmd.Parameters.AddWithValue("@from", fromDate);
                cmd.Parameters.AddWithValue("@to", toDate.AddDays(1).AddSeconds(-1));

                    //gets data
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                     //holds the data then show it sa gridview
                     DataTable dt = new DataTable(); 

                try
                {
                    conn.Open();
                    da.Fill(dt); //use for report no changes 
                    //close the database connection immediately

                    // Title Case the names (e.g., JUNJIE -> Junjie)
                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                    foreach (DataRow row in dt.Rows)
                    {
                        row["FromName"] = textInfo.ToTitleCase(row["FromName"].ToString().ToLower());
                        row["ToName"] = textInfo.ToTitleCase(row["ToName"].ToString().ToLower());
                    }

                    gvReports.DataSource = dt; //link the container sa website table
                    gvReports.DataBind(); //data on screen
                }
                catch (Exception ex)
                {
                    // This will show exactly what's wrong if the SQL fails
                    Response.Write("<script>alert('Error: " + ex.Message.Replace("'", "") + "');</script>");
                }
            }
        }
    }
}
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
    public partial class Forgot_Password : System.Web.UI.Page
    {
        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected void btnFind_Click(object sender, EventArgs e)
        {
            // Search the OS Database for the Username
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT COUNT(*) FROM Useraccount WHERE Username = @u";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", txtSearchUser.Text.Trim());

                conn.Open();
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    pnlSearch.Visible = false; // Hide the search
                    pnlVerify.Visible = true;  // Show the reset fields
                }
                else
                {
                    RunSweetAlert("Not Found", "That username does not exist.", "error");
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // 1. Final Password Match Check
            if (txtNewPass.Text != txtConfirm.Text)
            {
                RunSweetAlert("Mismatch", "Passwords do not match!", "warning");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();


                // 2. SILENT CHECK: Fetch the current password first
                string fetchSql = "SELECT Password FROM Useraccount WHERE Username = @u";
                string existingPassword = "";

                using (SqlCommand fetchCmd = new SqlCommand(fetchSql, conn))
                {
                    fetchCmd.Parameters.AddWithValue("@u", txtSearchUser.Text.Trim());
                    object result = fetchCmd.ExecuteScalar();
                    existingPassword = result != null ? result.ToString() : "";
                }

                // 3. Comparison Logic: Prevent reuse
                if (txtNewPass.Text.Trim() == existingPassword)
                {
                    RunSweetAlert("Security Policy", "New password cannot be the same as your current password.", "warning");
                    return; // Stop the execution here
                }

                // 2. The SQL Command: Update ONLY if Username AND BackupCode match
                // This is your security "Lock and Key"
                string sql = "UPDATE Useraccount SET Password = @newP " +
                             "WHERE Username = @u AND BackupCode = @b";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // We get @u from the search textbox used in Step 1
                    cmd.Parameters.AddWithValue("@u", txtSearchUser.Text.Trim());
                    cmd.Parameters.AddWithValue("@b", txtBackupVerify.Text.Trim());
                    cmd.Parameters.AddWithValue("@newP", txtNewPass.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // 3. Success!
                        string successScript = @"
                    Swal.fire({
                        title: 'Password Reset!',
                        text: 'Your account is secure. Please log in with your new password.',
                        icon: 'success',
                        confirmButtonColor: '#0D1B3E'
                    }).then(() => {
                        window.location.href = 'USER Login.aspx';
                    });";

                        ClientScript.RegisterStartupScript(this.GetType(), "resetSuccess", successScript, true);
                    }
                    else
                    {
                        // If rowsAffected is 0, it means the Backup Code was wrong
                        RunSweetAlert("Denied", "Invalid Recovery Code. Please try again.", "error");
                    }
                }
            }
        }
    }
}
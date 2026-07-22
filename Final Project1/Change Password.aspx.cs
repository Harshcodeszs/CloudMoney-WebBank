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
    public partial class Change_Password : System.Web.UI.Page
    {
        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Security Gate: Ensure the user is logged in
            if (Session["Username"] == null)
            {
                Response.Redirect("USER Login.aspx");
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // 1. Check if New and Confirm match
            if (txtNewPass.Text != txtConfirm.Text)
            {
                RunSweetAlert("Mismatch", "New passwords do not match or are empty!", "error");
                return;
            }

            string loggedInUser = Session["Username"].ToString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 2. UPDATE where both Username and Old Password are correct
                string sql = "UPDATE Useraccount SET Password = @newP " +
                             "WHERE Username = @u AND Password = @oldP";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@newP", txtNewPass.Text.Trim());
                    cmd.Parameters.AddWithValue("@u", loggedInUser);
                    cmd.Parameters.AddWithValue("@oldP", txtOldPass.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // 3. Success! Send them back to Login to try the new password
                        string successScript = @"
                    Swal.fire({
                        title: 'Success!',
                        text: 'Password updated! Please login with your new credentials.',
                        icon: 'success',
                        confirmButtonColor: '#0D1B3E'
                    }).then((result) => {
                        window.location.href = 'USER Login.aspx';
                    });";

                        ClientScript.RegisterStartupScript(this.GetType(), "updateSuccess", successScript, true);
                    }
                    else
                    {
                        RunSweetAlert("Error", "Invalid Username or Current Password.", "error");
                    }
                }
            }
        }
    }
}
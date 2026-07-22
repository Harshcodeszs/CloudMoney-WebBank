using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//namespaces
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace Final_Project1
{
    public partial class REGISTRATION : System.Web.UI.Page
    {

        //connection string
        string connStr = WebConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //clear textboxes
        private void ClearForm()
        {
            txtFname.Text = "";
            txtLname.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtCpassword.Text = "";
            txtBackup.Text = "";
        }

        //sweetalert
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
        protected void Register_Click(object sender, EventArgs e)
        {

            string recoveryCode = txtBackup.Text.Trim();
            // 1. Validation Check: Do the passwords match?
            if (txtPassword.Text != txtCpassword.Text)
            {
                RunSweetAlert("Oops!", "Passwords do not match!", "error");
                return; // exit
            }

            // 2. If they match, proceed to Database
            //connection closes if there is an error
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open(); //open current loc

                // 3. Username duplication
                string checkSql = "SELECT COUNT(*) FROM Useraccount WHERE Username = @u";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());

                    // ExecuteScalar returns the count (0 if free, 1 or more if taken)
                    //gets the single number result of the count
                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        RunSweetAlert("Username Taken", "Please choose a different username.", "warning"); 
                        return; // Stop the registration if the username is already in the database
                    }
                }
                //saving the acc
                string sql = "INSERT INTO Useraccount (FirstName, LastName, Username, Password, BackupCode, DateRegistered) " +
                     "VALUES (@fn, @ln, @u, @p, @b, @dr)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //parameters 
                    //prevent sql injection
                    cmd.Parameters.AddWithValue("@fn", txtFname.Text.Trim());
                    cmd.Parameters.AddWithValue("@ln", txtLname.Text.Trim());
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());


                    cmd.Parameters.AddWithValue("@b", txtBackup.Text.Trim());
                    cmd.Parameters.AddWithValue("@dr", DateTime.Now);


                    //used for INSERT, UPDATE, and DELETE
                    cmd.ExecuteNonQuery();


                    //sweetalert notif
                    string successScript = $@"
                    Swal.fire({{
                        title: 'Account Created!',
                        html: '<div style=""margin-bottom:15px;"">Welcome to CloudMoney! Your 6-digit Recovery Code is:</div>' +
                              '<div style=""font-size: 28px; font-weight: bold; color: #1E3A8A; background: #e2e8f0; padding: 10px; border-radius: 8px; letter-spacing: 5px; margin-bottom: 15px;"">{recoveryCode}</div>' +
                              '<div style=""color: #e53e3e; font-weight: bold; font-size: 14px;""> DON\'T FORGET TO SAVE THIS! DON\'T LOSE THE CODE OR YOU CANNOT RECOVER YOUR ACCOUNT.</div>',
                        icon: 'success',
                        confirmButtonText: 'I have saved it!',
                        confirmButtonColor: '#1E3A8A',
                        allowOutsideClick: false
                    }}).then((result) => {{
                        if (result.isConfirmed) {{
                            window.location.href = 'USER Login.aspx';
                        }}
                    }});";

                    //pops up before data
                    ClientScript.RegisterStartupScript(this.GetType(), "successRedirect", successScript, true);

                    ClearForm(); //reset textboxes
                }
            }
        }
    }
}
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
    public partial class LOG_IN : System.Web.UI.Page
    {
        //connection string
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
        protected void LogIn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                //1. search ready
                //user's details only if the Username and Password match exactly
                string sql = "SELECT AccountNo, FirstName, LastName, DateRegistered" +
                    " FROM Useraccount WHERE Username = @u AND Password = @p";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //parameters
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());

                    //read results row by row
                    SqlDataReader reader = cmd.ExecuteReader();

                    //2. verify
                    if (reader.Read())
                    {

                        //3.session login success
                        //load account info in dashboard
                        Session["AccNo"] = reader["AccountNo"].ToString();

                        Session["Username"] = txtUsername.Text.Trim();

                        //Title case sa ig log in (JIE TO Jie)
                        string fullName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((reader["FirstName"].ToString() + " " + reader["LastName"].ToString()).ToLower()); 
                        Session["UserFullName"] = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();

                        //4. data safety
                        // Check if the reader actually contains the date
                        //Check if DateRegistered is empty (NULL) in the database before converting it
                        if (reader["DateRegistered"] != DBNull.Value)
                        {
                            Session["RegDate"] = Convert.ToDateTime(reader["DateRegistered"]).ToShortDateString();
                        }
                        else
                        {
                            Session["RegDate"] = "Not Available";
                        }

                        //5. feedback then handling
                        string successScript = $@"
                    Swal.fire({{
                        title: 'Welcome Back!',
                        text: 'Logging in as {fullName}',
                        icon: 'success',
                        timer: 2000,
                        showConfirmButton: false
                    }}).then(() => {{
                        window.location.href = 'Dashboard.aspx';
                    }});";

                        //pops up before data
                        ClientScript.RegisterStartupScript(this.GetType(), "loginSuccess", successScript, true);
                    }
                    else
                    {
                       
                        RunSweetAlert("Access Denied", "Invalid Username or Password.", "error");
                    }
                }
            }
        }
    }
}
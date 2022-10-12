using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using E_Commerce.E_CommerceServiceReference;
using System.Text.RegularExpressions;
using WCFServer;
using System.Diagnostics;

namespace E_Commerce
{
    public partial class SignUp : Form
    {
        ServiceE_CommerceClient wcfClient;
        public SignUp()
        {
            InitializeComponent();
        }

        private void btnSignUp(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbName.Text) && !string.IsNullOrEmpty(tbSurname.Text) && !string.IsNullOrEmpty(tbEmail.Text) && !string.IsNullOrEmpty(tbPassword.Text))
            {
                wcfClient = new ServiceE_CommerceClient();

                try
                {
                    wcfClient.Connection();
                }
                catch (Exception)
                {
                    wcfClient.Close();
                    new ServerDown("signup", null,null, null, null, null, null).Show();
                    Hide();
                }

                if (tbEmail.Text.Contains("@ecommerce.com"))
                {
                    lblError.Visible = true;
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Choose another email!";
                }
                /* Uncomment for email validation
                else if (!new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(tbEmail.Text))
                {
                    lblError.Visible = true;
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Insert a valid email!";
                }
                */
                /* uncomment to have strong passwords only!
                else if (!new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$").IsMatch(tbPassword.Text))
                {
                    lblError.Visible = true;
                    lblError.ForeColor = Color.Red;
                    lblError.Text = "Password not strong!";
                }
                */
                else
                {
                    try
                    {
                        User u = new User(tbName.Text, tbSurname.Text, tbEmail.Text, tbPassword.Text, 0);
                        if (wcfClient.Sign_up(u))
                        {
                            lblError.Visible = true;
                            lblError.ForeColor = Color.Green;
                            lblError.Text = "Successfully registered!";
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.ForeColor = Color.Red;
                            lblError.Text = "User already exists";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Visible = true;
                        lblError.ForeColor = Color.Red;
                        lblError.Text = ex.ToString();
                    }
                }
                wcfClient.Close();
            }
            else
            {
                lblError.Visible = true;
                lblError.ForeColor = Color.Red;
                lblError.Text = "Please insert all fields";
            }
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("signup", null, null, null, null, null, null).Show();
                Hide();
            }
            wcfClient.Close();

            lblError.Hide();
        }

        private void password(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbPassword.Text))
            {
                tbPassword.UseSystemPasswordChar = true;
                if (tbPassword.Text == "Insert here your password" && tbPassword.Text == "Insert here your password")
                {
                    tbPassword.UseSystemPasswordChar = false;
                }
            }
        }

        private void lnkLogin(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Login().Show();
            Hide();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

      
        private void HomePage_FormClosing(object sender, FormClosingEventArgs e)
        {
             foreach (var process in Process.GetProcessesByName("sqlservr"))
            {
                process.Kill();
            }
            Environment.Exit(0);
        }
    }
}

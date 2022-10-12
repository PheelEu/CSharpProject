using E_Commerce.E_CommerceServiceReference;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WCFServer;

namespace E_Commerce
{
    public partial class Login : Form
    {
        ServiceE_CommerceClient wcfClient;
        public Login()
        {
            InitializeComponent();
        }
        private void btnLogin(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbEmail.Text) && !string.IsNullOrEmpty(tbPassword.Text))
            {
                wcfClient = new ServiceE_CommerceClient();

                try
                {
                    wcfClient.Connection();
                }
                catch (Exception)
                {
                    new ServerDown("login", null, null, null, null, null, null).Show();
                    Hide();
                }

                if (tbEmail.Text.Contains("@ecommerce.com"))
                {
                    try
                    {
                        Admin admin = wcfClient.LoginAdmin(tbEmail.Text, tbPassword.Text);
                        if (admin != null)
                        {
                            new WelcomePage(admin).Show();
                            Hide();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.ForeColor = Color.Red;
                            lblError.Text = "Wrong email and/or password";
                        }
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        User user = wcfClient.LoginUser(tbEmail.Text, tbPassword.Text);
                        if (user != null)
                        {
                            new HomePage(user).Show();
                            Hide();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.ForeColor = Color.Red;
                            lblError.Text = "Wrong email and/or password";
                        }
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.ForeColor = Color.Red;
                lblError.Text = "Please insert email/password";
            }

            wcfClient.Close();
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


        private void lnkRegister(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SignUp().Show();
            Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                new ServerDown("login", null, null, null, null, null, null).Show();
                Hide();
            }
            wcfClient.Close();

            lblError.Hide();
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

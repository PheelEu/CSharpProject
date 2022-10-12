using E_Commerce.E_CommerceServiceReference;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WCFServer;


namespace E_Commerce
{
    public partial class Profile : Form
    {
        private readonly User user;
        private readonly Address address;

        ServiceE_CommerceClient wcfClient;

        public Profile(User user, Address address)
        {
            this.user = user;
            this.address = address;
            InitializeComponent();
       
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            LblInfo.Visible = false;

            wcfClient = new ServiceE_CommerceClient();

            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                new ServerDown("Profile", user, null, null, null, null, address).Show();
                Hide();
            }
            LblUserName.Text = user.name;
            LblAmount.Text = user.wallet.ToString("c2");

            wcfClient = new ServiceE_CommerceClient();
            try
            {
                Address address = wcfClient.GetAddress(user);
                if (address != null)
                {
                    TbCountry.Text = address.country;
                    TbCity.Text = address.city;
                    TbPostCode.Text = address.postCode.ToString();
                    TbAddress.Text = address.address;
                }
                else
                {
                    LblInfo.Visible = true;
                    LblInfo.ForeColor = Color.Red;
                    LblInfo.Text = "No address found\nInsert a new one!";
                }
            }
            catch (Exception)
            {
                new ServerDown("ViewProfile", user, null, null, null, null, address).Show();
                Hide();
            }
        }


        private void HomePage_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("sqlservr"))
            {
                process.Kill();
            }
            Environment.Exit(0);
        }
        private void BtnGoBack_Click(object sender, EventArgs e)
        {
            new HomePage(user).Show();
            Hide();
        }

        private void BtnUpdateAddress_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TbCountry.Text) && !string.IsNullOrEmpty(TbCity.Text)
                && !string.IsNullOrEmpty(TbPostCode.Text) && !string.IsNullOrEmpty(TbAddress.Text))
            {
                wcfClient = new ServiceE_CommerceClient();

                try
                {
                    wcfClient.Connection();
                }
                catch (Exception)
                {
                    wcfClient.Close();
                    new ServerDown("Profile", user, null, null, null, null, address).Show();
                    Hide();
                }
                if (!new Regex(@"^\d+$").IsMatch(TbPostCode.Text))
                {
                    LblInfo.Visible = true;
                    LblInfo.ForeColor = Color.Red;
                    LblInfo.Text = "Insert a valid Postal Code!";
                }
                else
                {
                    try
                    {
                        if (address != null)
                        {
                            if (address.country == TbCountry.Text && address.city == TbCity.Text
                                && address.postCode == int.Parse(TbPostCode.Text) && address.address == TbAddress.Text)
                            {
                                LblInfo.Visible = true;
                                LblInfo.ForeColor = Color.Blue;
                                LblInfo.Text = "All the info are already up to date!";
                            }
                            else
                            {
                                try
                                {
                                    Address a = new Address(user.email, TbCountry.Text, TbCity.Text, int.Parse(TbPostCode.Text), TbAddress.Text);
                                    if (wcfClient.UpdateAddress(a, user))
                                    {
                                        LblInfo.Visible = true;
                                        LblInfo.ForeColor = Color.Green;
                                        LblInfo.Text = "Address updated!";
                                    }
                                    else
                                    {
                                        LblInfo.Visible = true;
                                        LblInfo.ForeColor = Color.Red;
                                        LblInfo.Text = "Error!\nAddress was not updated!";
                                    }
                                }
                                catch(Exception ex)
                                {
                                    LblInfo.Visible = true;
                                    LblInfo.ForeColor = Color.Red;
                                    LblInfo.Text = ex.ToString();
                                }
                            }
                                
                        }
                        else
                        {
                            Address a = new Address(user.email, TbCountry.Text, TbCity.Text, int.Parse(TbPostCode.Text), TbAddress.Text);
                            if (wcfClient.AddAddress(a, user))
                            {
                                LblInfo.Visible = true;
                                LblInfo.ForeColor = Color.Green;
                                LblInfo.Text = "Successfully registered!";
                            }
                            else
                            {
                                LblInfo.Visible = true;
                                LblInfo.ForeColor = Color.Red;
                                LblInfo.Text = "Error!\nAddress was not registered";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LblInfo.Visible = true;
                        LblInfo.ForeColor = Color.Red;
                        LblInfo.Text = ex.ToString();
                    }
                }
                wcfClient.Close();
            }
            else
            {
                LblInfo.Visible = true;
                LblInfo.ForeColor = Color.Red;
                LblInfo.Text = "Please insert all fields";
            }
        }

        private void BtnTopUp_Click(object sender, EventArgs e)
        {
            if (NumericUpDown.Value != 0) {
                wcfClient = new ServiceE_CommerceClient();

                try
                {
                    wcfClient.Connection();
                }
                catch (Exception)
                {
                    wcfClient.Close();
                    new ServerDown("Profile", user, null, null, null, null, address).Show();
                    Hide();
                }
                if (wcfClient.UpdateUserWallet(user, float.Parse(NumericUpDown.Value.ToString()), true))
                {
                    List<User> users = new List<User>();
                    foreach (User u in wcfClient.GetUsers())
                        if(user.email == u.email) {
                            user.wallet += float.Parse(NumericUpDown.Value.ToString());
                            LblAmount.Text =  u.wallet.ToString("c2");
                            LblInfo.Visible = true;
                            LblInfo.ForeColor = Color.Green;
                            LblInfo.Text = "The top up was successfull!";
                        }
                }
                else
                {
                    LblInfo.Visible = true;
                    LblInfo.ForeColor = Color.Red;
                    LblInfo.Text = "Error!\nWallet not updated";
                }
                wcfClient.Close();
            }
            else
            {
                LblInfo.Visible = true;
                LblInfo.ForeColor = Color.Red;
                LblInfo.Text = "Error!\nValue must be greater than 0!";
            }
        }
    }
}

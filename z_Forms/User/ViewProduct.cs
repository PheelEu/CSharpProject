using E_Commerce.E_CommerceServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCFServer;

namespace E_Commerce
{
    public partial class ViewProduct : Form
    { 
        private readonly Product product;
        private readonly User user;

        ServiceE_CommerceClient wcfClient;

        public ViewProduct(Product product, User user)
        {
            InitializeComponent();
            this.product = product;
            this.user = user;
        }

        private void BtnGoBack_Click(object sender, EventArgs e)
        {
            new HomePage(user).Show();
            Hide();
        }

        private void ViewProduct_Load(object sender, EventArgs e)
        { 
            lblName.Text = product.name;
            Image img = Image.FromFile("C:\\Users\\tensu\\source\\repos\\E-Commerce\\E-Commerce\\Resources\\" + product.img);
            img = new Bitmap(img, new Size(200, 150));
            PanelImage.BackgroundImage = img;
            PanelImage.Size = new Size(200, 150);
            lblDescription.Text = product.description;
            lblBrand.Text = product.brand;
            lblQuantity.Text = product.quantity.ToString();
            lblPrice.Text = product.price.ToString();

            numericUpDown.Maximum = product.quantity;

            if (product.quantity <= 0)
            {
                btnBuyProduct.Enabled = false;
                numericUpDown.Value=0;
                lblInfo.Visible = true;
                lblInfo.ForeColor = Color.Red;
                lblInfo.Text = "Item out of stock!";
            }
            else
            {
                btnBuyProduct.Enabled = true;
                lblInfo.Visible = false;
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

        private void BtnBuyProduct_Click(object sender, EventArgs e)
        {
            lblInfo.Visible=false;
            if (numericUpDown.Value <= product.quantity && numericUpDown.Value != 0
                && user.wallet >= (product.price*(int.Parse(numericUpDown.Value.ToString()))))
            {
                wcfClient = new ServiceE_CommerceClient();
                try
                {
                    wcfClient.Connection();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); 
                    new ServerDown("ViewProduct", user, null, null, product, null, null).Show();
                    Hide();
                }
                try
                {
                    Address address = wcfClient.GetAddress(user);
                    if (address != null)
                    {
                        try
                        {
                            bool isOrderSent = wcfClient.BuyProduct(product, user, int.Parse(numericUpDown.Value.ToString()), address);
                            if (isOrderSent)
                            {
                                lblInfo.Visible = true;
                                lblInfo.ForeColor = Color.Green;
                                lblInfo.Text = "Order sent!";
                                user.wallet -= (float)(int.Parse(numericUpDown.Value.ToString())*product.price);
                                product.quantity -= int.Parse(numericUpDown.Value.ToString());
                            }
                            else
                            {
                                lblInfo.Visible = true;
                                lblInfo.ForeColor = Color.Red;
                                lblInfo.Text = "There was an unknown issue\nContact an administrator!";
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            new ServerDown("ViewProduct", user, null, null, product, null, null).Show();
                            Hide();
                        }
                    }
                    else
                    {
                        lblInfo.Visible = true;
                        lblInfo.ForeColor = Color.Blue;
                        lblInfo.Text = "No valid address found\nGo to the profile page and add an address first!";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    new ServerDown("ViewProduct", user, null, null, product, null, null).Show();
                    Hide();
                }
                wcfClient.Close();
            }
            if (user.wallet < (product.price * (int.Parse(numericUpDown.Value.ToString()))))
            {
                lblInfo.Visible = true;
                lblInfo.ForeColor = Color.Red;
                lblInfo.Text = "Not enough juice in your wallet!\nTop it up in the profile page!";
            }
        }
    }
}

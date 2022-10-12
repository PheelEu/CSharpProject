using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WCFServer;
using E_Commerce.E_CommerceServiceReference;
using System.IO;
using Image = System.Drawing.Image;

namespace E_Commerce
{
    public partial class WelcomePage : Form
    {
        ServiceE_CommerceClient wcfClient;

        private readonly Admin admin;

        private Button btnAddProduct;
        private TextBox tbImage, tbName, tbBrand, tbDescription, tbPrice, tbQuantity;
        
        public WelcomePage(Admin admin)
        { 
            this.admin = admin;
            InitializeComponent();
        }

        private Label Labels(Size s, Point p, string text, Font f)
        {
            Label lbl = new Label
            {
                Size = s,
                Location = p,
                Text = text,
                ForeColor = Color.Black,
                Font = f,
                TextAlign = ContentAlignment.MiddleCenter
            };
            return lbl;
        }
        private Panel Panels(Size s, Point p, BorderStyle b)
        {
            Panel x = new Panel
            {
                Size = s,
                Location = p,
                BorderStyle = b
            };
            return x;
        }
        private TextBox TextBoxAddProduct(Size s, Point p)
        {
            TextBox tb = new TextBox
            {
                Size = s,
                Location = p
            };
            return tb;
        }
        private Button Buttons(Size s, Point p, Color bc, Font f, Color fc)
        {
            Button b = new Button
            {
                Size = s,
                Location = p,
                BackColor = bc,
                Font = f,
                ForeColor = fc
            };
            return b;
        }

        private void SetScrollFalse()
        {
            panel.AutoScroll = false;
            panel.HorizontalScroll.Visible = false;
            panel.HorizontalScroll.Enabled = false;
            panel.HorizontalScroll.Value = 0;
            panel.HorizontalScroll.Maximum = 0;
            panel.HorizontalScroll.Minimum = 0;
            panel.VerticalScroll.Visible = false;
            panel.VerticalScroll.Enabled = true;
            panel.VerticalScroll.Value = 0;
            panel.VerticalScroll.Maximum = 0;
            panel.VerticalScroll.Minimum = 0;
            panel.AutoScroll = true;
        }
        private void SetAddButtons()
        {
            btnAddProduct = Buttons(new Size(150, 30), new Point(800, 50), Color.Blue, new Font("Arial", 10F), Color.White);
            btnAddProduct.Visible = false;
            btnAddProduct.Click += AddProduct_Click;
            Controls.Add(btnAddProduct);
        }

        private void CreateProduct()
        {
            panel.Controls.Clear();
            panel.Update();

            Panel px = CreatePanelAddProduct();
            panel.Controls.Add(px);

            int y = panel.Size.Height / 5;

            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("WelcomePage", null, admin, null, null, null, null).Show();
                Hide();
            }
           
            foreach (Product pr in wcfClient.GetProducts())
            {
                Panel p = CreatePanelProduct(y, pr);
                panel.Controls.Add(p);
                y += panel.Size.Height / 5;
            }

            wcfClient.Close();
        }

        private void CreateOrder()
        {
            panel.Controls.Clear();
            panel.Update();

            
            int x = panel.Size.Width, y = panel.Size.Height;
            int width = x / 3, height = y / 4;
            int counterX = 0, counterY = 0;

            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("WelcomePage", null, admin, null, null, null, null).Show();
                Hide();
            }

            foreach (Order o in wcfClient.GetOrders())
            {
            
                Panel p = new Panel
                {
                    Location = new Point(counterX * width, counterY * height),
                    Size = new Size(width, height)
                };

                Label lblInfo = Labels(new Size(width, height), new Point(0, 25), "Order ID: " + o.orderId +
                    "\nUser Email: " + o.user_email + "\nProduct: " + o.product.name + "\nQuantity: " + o.quantity + "\nPrice: " + o.price +
                    "\nAddress: " + o.address + "\nCity: " + o.city + "\nOrder Date:" + o.orderDate.ToString("yyyy-MM-dd HH:mm"), null);
                Label lblHeaderInfo = Labels(new Size(width, 25), new Point(0, 15), "Order details:", new Font("Arial", 8, FontStyle.Bold));
                lblHeaderInfo.ForeColor = Color.Blue;

                p.Controls.Add(lblHeaderInfo);
                p.Controls.Add(lblInfo);

                counterX++;
                if (counterX == 3)
                {
                    counterY++;
                }
                if (counterX % 3 == 0)
                {
                    counterX = 0;
                }

                panel.Controls.Add(p);
                p.ResumeLayout();

            }
            wcfClient.Close();
        }

        private void CreatePanelUsers()
        {
            int x = panel.Size.Width, y = panel.Size.Height;
            int width = x / 3, height = y / 4;
            int counterX = 0, counterY = 0;

            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("WelcomePage", null, admin, null, null, null, null).Show();
                Hide();
            }

            foreach (User user in wcfClient.GetUsers())
            {
                Panel p = new Panel
                {
                    Location = new Point(counterX * width, counterY * height),
                    Size = new Size(width, height)
                };

                Label lblInfo = Labels(new Size(width, height), new Point(0, 25), "Name: " + user.name +
                    "\nSurname: " + user.surname + "\nEmail: " + user.email + "\nPassword: " + user.password + "\nWallet: " + user.wallet, null);
                Label lblHeaderInfo = Labels(new Size(width, 25), new Point(0, 15), "User details:", new Font("Arial", 10, FontStyle.Bold));
                lblHeaderInfo.ForeColor = Color.Blue;

                p.Controls.Add(lblHeaderInfo);
                p.Controls.Add(lblInfo);

                counterX++;
                if (counterX == 3)
                {
                    counterY++;
                }
                if (counterX % 3 == 0)
                {
                    counterX = 0;
                }

                panel.Controls.Add(p);
                p.ResumeLayout();
            }
            wcfClient.Close();
        }

        
        private Panel CreatePanelAddProduct()
        {
            Panel p = Panels(new Size(panel.Size.Width, panel.Size.Height / 5), new Point(0, 0), BorderStyle.FixedSingle);

            Button btnImage = Buttons(new Size(80, 40), new Point(30, 5), Color.White, new Font("Arial", 10F), Color.Black);
            btnImage.Text = "+Image";
            btnImage.Click += btnImage_Click;
            tbImage = TextBoxAddProduct(new Size(100, 30), new Point(20, 60));
            Label lblName = Labels(new Size(100, 30), new Point(130, 10), "Insert name", new System.Drawing.Font("Arial", 10F));
            Label lblBrand = Labels(new Size(100, 30), new Point(240, 10), "Insert brand", new System.Drawing.Font("Arial", 10F));
            Label lblDescription = Labels(new Size(150, 30), new Point(350, 10), "Insert description", new System.Drawing.Font("Arial", 10F));
            Label lblQuantity = Labels(new Size(100, 30), new Point(510, 10), "Insert quantity", new System.Drawing.Font("Arial", 10F));
            Label lblPrice = Labels(new Size(100, 30), new Point(620, 10), "Insert price", new System.Drawing.Font("Arial", 10F));
            tbName = TextBoxAddProduct(new Size(100, 30), new Point(130, 60));
            tbBrand = TextBoxAddProduct(new Size(100, 30), new Point(240, 60));
            tbDescription = TextBoxAddProduct(new Size(150, 30), new Point(350, 60));
            tbQuantity = TextBoxAddProduct(new Size(100, 30), new Point(510, 60));
            tbPrice = TextBoxAddProduct(new Size(100, 30), new Point(620, 60));

            Control[] addControls = new Control[] { btnImage, tbImage, lblName, tbName, lblBrand, tbBrand, lblDescription, tbDescription, lblQuantity, tbQuantity, lblPrice, tbPrice };
            p.Controls.AddRange(addControls);
            return p;
        }
        
        
        private Panel CreatePanelProduct(int y, Product pr)
        {
            Panel p = Panels(new Size(panel.Size.Width, panel.Size.Height / 5), new Point(0, y), BorderStyle.FixedSingle);
            Image img = Image.FromFile(WCFServer.Program.GetUrl() + pr.img);
            img = new Bitmap(img, new Size(80, 50));
            Panel pImage = Panels(new Size(80, 50), new Point(20, 10), BorderStyle.None);
            pImage.BackgroundImage = img;
            Label lblName = Labels(new Size(100, 30), new Point(110, 30), pr.name, null);
            Label lblBrand = Labels(new Size(100, 30), new Point(220, 30), pr.brand, null);
            Label lblDescription = Labels(new Size(150, 30), new Point(330, 30), pr.description, null);
            Label lblQuantity = Labels(new Size(100, 30), new Point(490, 30), pr.quantity.ToString(), null);
            Label lblPrice = Labels(new Size(100, 30), new Point(600, 30), pr.price.ToString(), null);
            Panel pButton = Panels(new Size(90, 35), new Point(710, 30), BorderStyle.None);
            Button btnRemove = Buttons(new Size(90, 35), new Point(0, 0), Color.Blue, new Font("Arial", 10F), Color.White);
            btnRemove.Text = "Delete";
            btnRemove.Tag = pr;
            btnRemove.Click += DeleteProduct_Click;

            pButton.Controls.Add(btnRemove);

            Control[] addControls = new Control[] { pImage, lblName, lblBrand, lblDescription, lblQuantity, lblPrice, pButton };
            p.Controls.AddRange(addControls);
            return p;
        }

        private Panel CreatePanelOrder(int y, Order o)
        {
            Panel p = Panels(new Size(panel.Size.Width, panel.Size.Height / 5), new Point(0, y), BorderStyle.FixedSingle);
            Image img = Image.FromFile(WCFServer.Program.GetUrl() + o.product.img);
            img = new Bitmap(img, new Size(50, 80));
            Panel pImage = Panels(new Size(50, 80), new Point(50, 10), BorderStyle.None);
            pImage.BackgroundImage = img;
            Label lblName = Labels(new Size(100, 80), new Point(100, 10), o.product.name, null);
            Label lblUserEmail = Labels(new Size(150, 80), new Point(250, 10), o.user_email, null);
            Label lblQuantity = Labels(new Size(100, 80), new Point(350, 10), "Quantity: " + o.quantity, null);
            Label lblPrice = Labels(new Size(100, 80), new Point(450, 10), "Price: " + o.price, null);
            Label lblAddress = Labels(new Size(400, 80), new Point(850, 10), "Address: " + o.address + ", City: " + o.city, null);
            Label lblDate = Labels(new Size(150, 80), new Point(1000, 10), "Order date: " + o.orderDate.ToString("yyyy-MM-dd HH:mm"), null);

            Control[] addControls = new Control[] { pImage, lblName, lblUserEmail, lblQuantity, lblPrice, lblAddress, lblDate };
            p.Controls.AddRange(addControls);
            return p;
        }

        private void BtnProducts_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Update();
            CreateProduct();
            btnAddProduct.Text = "Add Product";
            btnAddProduct.Visible = true;
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Update();
            CreateOrder();
            btnAddProduct.Visible = false;
        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            panel.Controls.Clear();
            panel.Update();
            CreatePanelUsers();
            btnAddProduct.Visible = false;
        }

        private void LblWelcome_Click(object sender, EventArgs e)
        {
            new Login().Show();
            Hide();
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = WCFServer.Program.GetUrl();
                ofd.Filter = "Image Files| *.jpg; *.jpeg; *.png; *.gif; *.tif| All files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName;
                    tbImage.Text = Path.GetFileName(path);
                }
            }
        }

        private void AddProduct_Click(object sender, EventArgs e)
        {
            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("WelcomePage", null, admin, null, null, null, null).Show();
                Hide();
            }
            if (!string.IsNullOrEmpty(tbImage.Text) && !string.IsNullOrEmpty(tbName.Text) && !string.IsNullOrEmpty(tbBrand.Text)
                && !string.IsNullOrEmpty(tbDescription.Text) && !string.IsNullOrEmpty(tbQuantity.Text) && !string.IsNullOrEmpty(tbPrice.Text))
            {
                Product pr = new Product(tbName.Text, tbBrand.Text, tbDescription.Text, tbImage.Text, float.Parse(tbPrice.Text), int.Parse(tbQuantity.Text));
                try
                {
                    if (wcfClient.AddProduct(pr))
                    {
                        MessageBox.Show("Product added correctly");
                        CreateProduct();
                    }
                    else
                    {
                        MessageBox.Show("Product already exists");
                    }
                }
                catch (Exception) { }
            }
            else
            {
                MessageBox.Show("Insert all fields!");
            }
            wcfClient.Close();
        }

        private void DeleteProduct_Click(object sender, EventArgs e)
        {
            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("WelcomePage", null, admin, null, null, null, null).Show();
                Hide();
            }

            Product pr = null;
            if (sender.GetType().Name == "Button")
            {
                pr = (Product)((Button)sender).Tag;
            }

            if (wcfClient.DeleteProduct(pr))
            {
                MessageBox.Show("Product deleted correctly");
                CreateProduct();
            }
            else
            {
                MessageBox.Show("Could not delete product!");
            }
        }


        private void WelcomePage_Load(object sender, EventArgs e)
        {
            SetScrollFalse();
            SetAddButtons();
            lblWelcome.Text = "Welcome " + admin.email + "\nClick here to logout!";
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

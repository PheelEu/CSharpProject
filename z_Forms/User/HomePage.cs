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
using E_Commerce.E_CommerceServiceReference;
using WCFServer;

namespace E_Commerce
{
    public partial class HomePage : Form
    {
        ServiceE_CommerceClient wcfClient;

        private readonly User user;

        private List<Product> products;
        public HomePage(User user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void CreatePanelProduct(List<Product> products)
        {
            int x = panel.Size.Width, y = panel.Size.Height;
            int width = x / 4, height = y / 3;
            int counterX = 0, counterY = 0;

            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("HomePage", user, null, null, null, null, null).Show();
                Hide();
            }
            foreach (Product pr in wcfClient.GetProducts())
            {
                Panel p = CreatePanel(counterX * width, counterY * height, width, height, pr);
                counterX++;
                if (counterX == 4)
                {
                    counterY++;
                }
                if (counterX % 4 == 0)
                {
                    counterX = 0;
                }

                panel.Controls.Add(p);
                p.ResumeLayout();
            }
            wcfClient.Close();
        }

        private Panel CreatePanel(int x, int y, int w, int h, Product pr)
        {
            Panel p = new Panel
            {
                Size = new Size(w, h),
                Location = new Point(x, y),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = (Object)pr.name
            };

            Image img = Image.FromFile("C:\\Users\\tensu\\source\\repos\\E-Commerce\\E-Commerce\\Resources\\" + pr.img);
            img = new Bitmap(img, new Size(225, 155));

            Panel pIn = PanelProduct(img, pr.name);

            Label lblName = lblProductName(pr.name);

            p.Controls.Add(pIn);
            p.Controls.Add(lblName);

            lblName.Click += ShowProduct;
            pIn.Click += ShowProduct;
            p.Click += ShowProduct;
            return p;
        }

        private Panel PanelProduct(Image img, string title)
        {
            Panel p = new Panel
            {
                Size = new Size(225, 155),
                Location = new Point(10, 10),
                BackgroundImage = img,
                Tag = (Object)title
            };
            return p;
        }

        private Label lblProductName(string name)
        {
            Label lblName = new Label
            {
                Text = name,
                Size = new Size(150, 35),
                Location = new Point(55, 170),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10F, FontStyle.Bold),
                Tag = (Object)name
            };
            return lblName;
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

       

        private void ShowProduct(object sender, EventArgs e)
        {
            Product product = new Product();

            string name = "";
            if (sender.GetType().Name == "Label")
            {
                name = ((Label)sender).Tag.ToString();
            }
            if (sender.GetType().Name == "Panel")
            {
                name = ((Panel)sender).Tag.ToString();
            }

            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("HomePage", user, null, null, null, null, null).Show();
                Hide();
            }
            foreach (Product pr in wcfClient.GetProducts())
            {
                if (pr.name == name)
                {
                    product = pr;
                }
            }

            new ViewProduct(product, user).Show();
            Hide();
            wcfClient.Close();
        }
   
        private void HomePage_Load(object sender, EventArgs e)
        {
            SetScrollFalse();

            lblWelcome.Text = "Welcome " + user.name + "\nClick here to logout!";

            CreatePanelProduct(products);
        }

        private void btnOrders(object sender, EventArgs e)
        {
            List<Order> orders = new List<Order>();
            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("HomePage", user, null, null, null, null, null).Show();
                Hide();
            }

            foreach (Order o in wcfClient.GetOrders())
            {
                if (o.user_email == user.email)
                {
                    orders.Add(o);
                }
            }
            wcfClient.Close();
         

            new ViewOrders(user, orders).Show();
            Hide();
        }

        private void btnProfile(object sender, EventArgs e)
        {
            wcfClient = new ServiceE_CommerceClient();
            try
            {
                wcfClient.Connection();
            }
            catch (Exception)
            {
                wcfClient.Close();
                new ServerDown("HomePage", user, null, null, null, null, null).Show();
                Hide();
            }
            Address address = wcfClient.GetAddress(user);
            new Profile(user, address).Show();
            Hide();
            wcfClient.Close();
        }
        private void HomePage_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("sqlservr"))
            {
                process.Kill();
            }
            Environment.Exit(0);
        }

        private void LblWelcome_Click(object sender, EventArgs e)
        {
            new Login().Show();
            Hide();
        }
    }
}

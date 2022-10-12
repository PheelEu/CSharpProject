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
    public partial class ViewOrders : Form
    {
        ServiceE_CommerceClient wcfClient;

        private User user;
        private List<Order> orders;

        public ViewOrders(User user, List<Order> orders)
        {
            this.user = user;
            this.orders = orders;
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

        private void Orders_Load(object sender, EventArgs e)
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
                new ServerDown("ViewOrders", user, null, null, null, orders, null).Show();
                Hide();
            }

            foreach (Order o in wcfClient.GetOrders())
            {
                if (o.user_email == user.email)
                {
                    Panel p = new Panel
                    {
                        Location = new Point(counterX * width, counterY * height),
                        Size = new Size(width, height)
                    };

                    Label lblInfo = Labels(new Size(width, height), new Point(0, 25), "Order ID: " + o.orderId +
                        "\nUser Email: " + o.user_email + "\nProduct: " + o.product.name + "\nQuantity: " + o.quantity + "\nPrice: " + o.price +
                        "\nAddress: " + o.address + "\nCity: " + o.city + "\nOrder Date:" + o.orderDate.ToString("yyyy-MM-dd HH:mm"), null);
                    Label lblHeaderInfo = Labels(new Size(width, 25), new Point(0, 15), "User details:", new Font("Arial", 8, FontStyle.Bold));
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
            }
            wcfClient.Close();
        }

        private void BtnGoBack_Click(object sender, EventArgs e)
        {
            new HomePage(user).Show();
            Hide();
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

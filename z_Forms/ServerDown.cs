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
    public partial class ServerDown : Form
    {
        private readonly string oldForm;
        private readonly User user;
        private readonly Admin admin;
        private readonly List<Product> products;
        private readonly Product product;
        private readonly List<Order> orders;
        private readonly Address address;

        public ServerDown(string oldForm, User user, Admin admin, List<Product> products, Product product, List<Order> orders,Address address)
        {
            InitializeComponent();
            this.oldForm = oldForm;
            this.user = user;
            this.admin = admin;
            this.products = products;
            this.product = product;
            this.orders = orders;
            this.address = address;
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            switch (oldForm)
            {
                case "login":
                    new Login().Show();
                    break;
                case "signup":
                    new SignUp().Show();
                    break;
                case "homepage":
                    new HomePage(user).Show();
                    break;
                case "welcomepage":
                    new WelcomePage(admin).Show();
                    break;
                case "ViewProduct":
                    new ViewProduct(product, user).Show();
                    break;
                case "ViewOrders":
                    new ViewOrders(user, orders).Show();
                    break;
                case "Profile":
                    new Profile(user, address).Show();
                    break;
               
            }
            Hide();
        }

        private void ServerDown_Load(object sender, EventArgs e)
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClothingStoreApp.Data;
using ClothingStoreApp.Models;

namespace ClothingStoreApp
{
    public partial class DashboardForm : MaterialForm
    {
        public Sale? GeneratedSale { get; private set; }

        private MaterialLabel? lblWelcome;
        private MaterialButton? btnLogout;
        private ListBox? lstProducts;
        private MaterialLabel? lblStats;
        private MaterialButton? btnManageProducts;
        private MaterialButton? btnPurchase;
        private MaterialButton? btnInvoiceHistory;


        private readonly AppDbContext _context = new AppDbContext();
        private List<Product> products = new List<Product>();

        public DashboardForm(string username, string role)
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue600, Primary.Blue700,
                Primary.Blue200, Accent.LightBlue200,
                TextShade.WHITE
            );
             
            this.Text = $"Clothify                                                                                      Welcome, {username}";
            this.Size = new Size(700, 500);

              

            lblStats = new MaterialLabel
            {
                Text = "Sales Stats:\n- Today: 5 Orders\n- This Week: 28 Orders",
                Location = new Point(60, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 30, FontStyle.Bold)
            };

            lstProducts = new ListBox
            {
                Location = new Point(60, 120),
                Size = new Size(300, 200),
                BackColor = Color.LightYellow // 🎨 اختار اللون اللي يعجبك
            };


            products = _context.Products.ToList();
            foreach (var product in products)
            {
                lstProducts.Items.Add($"{product.Name} - {product.Price} EGP ({product.Quantity} pcs)");
            }

            btnPurchase = new MaterialButton
            {
                Text = "New purchase",
                Location = new Point(450, 80),
                AutoSize = true
            };
            btnPurchase.Click += BtnPurchase_Click; // ✅ هذا هو الجزء المهم


            btnLogout = new MaterialButton
            {
                Text = "Logout",
                Location = new Point(550, 400),
                AutoSize = true
            };

            btnInvoiceHistory = new MaterialButton
            {
                Text = "Invoice History",
                Location = new Point(450, 130),
                AutoSize = true
            };
            btnInvoiceHistory.Click += BtnInvoiceHistory_Click;
            this.Controls.Add(btnInvoiceHistory);

            btnLogout.Click += BtnLogout_Click;

            btnManageProducts = new MaterialButton
            {
                Text = "Manage Products",
                Location = new Point(380, 400),
                AutoSize = true
            };
            btnManageProducts.Click += BtnManageProducts_Click;

            this.Controls.Add(lblWelcome);
            this.Controls.Add(lstProducts);
            this.Controls.Add(lblStats);
            this.Controls.Add(btnPurchase);
            this.Controls.Add(btnLogout);
            this.Controls.Add(btnManageProducts);
        }

        private void BtnInvoiceHistory_Click(object? sender, EventArgs e)
        {
            var form = new InvoiceHistoryForm();
            form.ShowDialog();
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void BtnManageProducts_Click(object? sender, EventArgs e)
        {
            ProductManagementForm form = new ProductManagementForm();
            form.ShowDialog();
        }

        private void BtnPurchase_Click(object? sender, EventArgs e)
        {
            // افتح الفورم مباشرة بدون التحقق من تحديد منتج
            var invoiceForm = new InvoiceForm(new List<Product>());

            if (invoiceForm.ShowDialog() == DialogResult.OK)
            {
                var sale = invoiceForm.GeneratedSale;
                if (sale != null)
                {
                    var viewForm = new InvoiceViewForm(sale);
                    viewForm.ShowDialog();
                }
            }
        }

    }
}


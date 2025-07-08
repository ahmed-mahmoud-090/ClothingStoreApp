using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClothingStoreApp.Data;
using ClothingStoreApp.Models;

namespace ClothingStoreApp
{
    public partial class InvoiceForm : MaterialForm
    {
        private readonly AppDbContext _context = new AppDbContext();
        private List<SaleItem> saleItems = new List<SaleItem>();
        public Sale? GeneratedSale { get; private set; }

        private ComboBox? cmbProducts;
        private NumericUpDown? nudQuantity;
        private Button? btnAddToInvoice;
        private ListBox? lstInvoiceItems;
        private Button? btnConfirm;
        private Button? btnBack;

        public InvoiceForm(List<Product> selectedList)
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Green600, Primary.Green700,
                Primary.Green200, Accent.LightGreen200,
                TextShade.WHITE
            );

            this.Text = "New Invoice";
            this.Size = new System.Drawing.Size(600, 500);

            SetupUI();
            LoadProducts();
        }

        private void SetupUI()
        {
            cmbProducts = new ComboBox
            {
                Location = new System.Drawing.Point(30, 80),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            nudQuantity = new NumericUpDown
            {
                Location = new System.Drawing.Point(300, 80),
                Width = 80,
                Minimum = 1,
                Maximum = 100
            };

            btnAddToInvoice = new Button
            {
                Text = "Add to Invoice",
                Location = new System.Drawing.Point(400, 78)
            };
            btnAddToInvoice.Click += BtnAddToInvoice_Click;

            lstInvoiceItems = new ListBox
            {
                Location = new System.Drawing.Point(30, 130),
                Size = new System.Drawing.Size(500, 200)
            };

            btnConfirm = new Button
            {
                Text = "Confirm Purchase",
                Location = new System.Drawing.Point(30, 350)
            };
            btnConfirm.Click += BtnConfirm_Click;

            btnBack = new Button
            {
                Text = "Back",
                Location = new System.Drawing.Point(160, 350)
            };
            btnBack.Click += (s, e) =>
            {
                this.Close();
            };

            this.Controls.Add(cmbProducts);
            this.Controls.Add(nudQuantity);
            this.Controls.Add(btnAddToInvoice);
            this.Controls.Add(lstInvoiceItems);
            this.Controls.Add(btnConfirm);
            this.Controls.Add(btnBack);
        }

        private void LoadProducts()
        {
            var products = _context.Products.ToList();
            cmbProducts.DataSource = products;
            cmbProducts.DisplayMember = "Name";
            cmbProducts.ValueMember = "Id";
        }

        private void BtnAddToInvoice_Click(object? sender, EventArgs e)
        {
            if (cmbProducts.SelectedItem is Product selectedProduct)
            {
                int quantity = (int)nudQuantity.Value;

                if (quantity > selectedProduct.Quantity)
                {
                    MessageBox.Show("Not enough quantity in stock!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal total = selectedProduct.Price * quantity;

                saleItems.Add(new SaleItem
                {
                    ProductId = selectedProduct.Id,
                    ProductName = selectedProduct.Name,
                    Quantity = quantity,
                    UnitPrice = selectedProduct.Price
                });

                lstInvoiceItems.Items.Add($"{selectedProduct.Name} x {quantity} = {total} EGP");
            }
        }

        private void BtnConfirm_Click(object? sender, EventArgs e)
        {
            if (saleItems.Count == 0)
            {
                MessageBox.Show("Please add items to invoice.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // خصم الكمية من المنتجات
            foreach (var item in saleItems)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    if (product.Quantity >= item.Quantity)
                    {
                        product.Quantity -= item.Quantity;
                    }
                    else
                    {
                        MessageBox.Show($"Not enough stock for {product.Name}.");
                        return;
                    }
                }
            }

            // إنشاء الفاتورة
            var sale = new Sale
            {
                Date = DateTime.Now,
                InvoiceCode = Guid.NewGuid().ToString().Substring(0, 8),
                TotalAmount = saleItems.Sum(i => i.Quantity * i.UnitPrice),
                Items = saleItems
            };

            _context.Sales.Add(sale);
            _context.SaveChanges();

            GeneratedSale = sale;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

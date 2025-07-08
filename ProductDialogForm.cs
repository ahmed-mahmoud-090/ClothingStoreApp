using System;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClothingStoreApp.Models;

namespace ClothingStoreApp
{
    public partial class ProductDialogForm : MaterialForm
    {
        private MaterialLabel? lblName;
        private MaterialLabel? lblPrice;
        private MaterialLabel? lblQuantity;
        private MaterialTextBox? txtName;
        private NumericUpDown? nudPrice;
        private NumericUpDown? nudQuantity;
        private MaterialButton? btnSave;
        private MaterialButton? btnCancel;

        public Product ProductData { get; private set; }

        public ProductDialogForm(Product? existingProduct = null)
        {
            // Material UI Setup
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Indigo500, Primary.Indigo700,
                Primary.Indigo100, Accent.Indigo200,
                TextShade.WHITE
            );

            this.Text = existingProduct == null ? "Add Product" : "Edit Product";
            this.Size = new System.Drawing.Size(400, 350);

            SetupUI();

            if (existingProduct != null)
            {
                txtName.Text = existingProduct.Name;
                nudPrice.Value = (decimal)existingProduct.Price;
                nudQuantity.Value = existingProduct.Quantity;

                ProductData = existingProduct;
            }
            else
            {
                ProductData = new Product();
            }
        }

        private void SetupUI()
        {
            lblName = new MaterialLabel { Text = "Product Name", Location = new System.Drawing.Point(30, 70) };
            txtName = new MaterialTextBox
            {
                Location = new System.Drawing.Point(30, 100),
                Width = 300,
                Hint = "e.g., T-Shirt"
            };

            lblPrice = new MaterialLabel { Text = "Price", Location = new System.Drawing.Point(30, 150) };
            nudPrice = new NumericUpDown
            {
                Location = new System.Drawing.Point(30, 180),
                Width = 120,
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 10000
            };

            lblQuantity = new MaterialLabel { Text = "Quantity", Location = new System.Drawing.Point(180, 150) };
            nudQuantity = new NumericUpDown
            {
                Location = new System.Drawing.Point(180, 180),
                Width = 120,
                Minimum = 1,
                Maximum = 1000
            };

            btnSave = new MaterialButton
            {
                Text = "Save",
                Location = new System.Drawing.Point(60, 240),
                AutoSize = true
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new MaterialButton
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(160, 240),
                AutoSize = true
            };
            btnCancel.Click += (s, e) => this.Close();

            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblPrice);
            Controls.Add(nudPrice);
            Controls.Add(lblQuantity);
            Controls.Add(nudQuantity);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter product name.");
                return;
            }

            ProductData.Name = txtName.Text.Trim();
            ProductData.Price = nudPrice.Value;
            ProductData.Quantity = (int)nudQuantity.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

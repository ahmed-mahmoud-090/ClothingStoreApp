using System;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClothingStoreApp.Data;
using ClothingStoreApp.Models;

namespace ClothingStoreApp
{
    public partial class ProductManagementForm : MaterialForm
    {
        private MaterialListView? productListView;
        private MaterialButton? btnAdd;
        private MaterialButton? btnEdit;
        private MaterialButton? btnDelete;
        private MaterialButton? btnBack;

        private readonly AppDbContext _context = new AppDbContext();

        public ProductManagementForm()
        {
            InitializeComponent();

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.DeepPurple600, Primary.DeepPurple700,
                Primary.DeepPurple200, Accent.DeepPurple200,
                TextShade.WHITE
            );

            this.Text = "Product Management";
            this.Size = new System.Drawing.Size(700, 500);

            SetupUI();
            LoadProducts();
        }

        private void SetupUI()
        {
            productListView = new MaterialListView
            {
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(640, 250),
                View = View.Details,
                FullRowSelect = true
            };
            productListView.Columns.Add("ID", 50);
            productListView.Columns.Add("Name", 200);
            productListView.Columns.Add("Price", 100);
            productListView.Columns.Add("Quantity", 100);

            btnAdd = new MaterialButton
            {
                Text = "Add",
                Location = new System.Drawing.Point(20, 350),
                AutoSize = true
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new MaterialButton
            {
                Text = "Edit",
                Location = new System.Drawing.Point(120, 350),
                AutoSize = true
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new MaterialButton
            {
                Text = "Delete",
                Location = new System.Drawing.Point(220, 350),
                AutoSize = true
            };
            btnDelete.Click += BtnDelete_Click;

            btnBack = new MaterialButton
            {
                Text = "Back",
                Location = new System.Drawing.Point(320, 350),
                AutoSize = true
            };
            btnBack.Click += BtnBack_Click;

            Controls.Add(productListView);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnBack);
        }

        private void LoadProducts()
        {
            productListView.Items.Clear();

            var products = _context.Products.ToList();
            foreach (var product in products)
            {
                var item = new ListViewItem(product.Id.ToString());
                item.SubItems.Add(product.Name);
                item.SubItems.Add(product.Price.ToString());
                item.SubItems.Add(product.Quantity.ToString());

                productListView.Items.Add(item);
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            var dialog = new ProductDialogForm();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _context.Products.Add(dialog.ProductData);
                _context.SaveChanges();
                LoadProducts();
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (productListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a product to edit.");
                return;
            }

            int productId = int.Parse(productListView.SelectedItems[0].Text);
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (existingProduct != null)
            {
                var dialog = new ProductDialogForm(existingProduct);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    existingProduct.Name = dialog.ProductData.Name;
                    existingProduct.Price = dialog.ProductData.Price;
                    existingProduct.Quantity = dialog.ProductData.Quantity;

                    _context.SaveChanges();
                    LoadProducts();
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (productListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this product?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                int productId = int.Parse(productListView.SelectedItems[0].Text);
                var product = _context.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                    LoadProducts();
                }
            }
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            this.Close(); // أو ارجع للشاشة الرئيسية حسب تصميمك
        }
    }
}

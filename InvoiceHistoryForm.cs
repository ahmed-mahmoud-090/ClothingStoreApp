using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using ClothingStoreApp.Data;
using ClothingStoreApp.Models;

namespace ClothingStoreApp
{
    public partial class InvoiceHistoryForm : Form
    {
        private readonly AppDbContext _context = new AppDbContext();
        private ListBox lstInvoices;
        private Button btnClose;
        private TextBox txtSearch;
        private Button btnSearch;

        public InvoiceHistoryForm()
        {
            this.Text = "Invoice History";
            this.Size = new System.Drawing.Size(600, 450);

            // ✅ Search TextBox
            txtSearch = new TextBox
            {
                PlaceholderText = "Enter Invoice Code",
                Location = new Point(20, 20),
                Width = 300
            };

            // ✅ Search Button
            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(330, 18)
            };
            btnSearch.Click += BtnSearch_Click;

            // ✅ ListBox
            lstInvoices = new ListBox
            {
                Location = new Point(20, 60),
                Size = new Size(540, 280)
            };
            lstInvoices.DoubleClick += LstInvoices_DoubleClick;

            // ✅ Close Button
            btnClose = new Button
            {
                Text = "Close",
                Location = new Point(20, 360)
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(txtSearch);
            this.Controls.Add(btnSearch);
            this.Controls.Add(lstInvoices);
            this.Controls.Add(btnClose);

            LoadInvoices();
        }

        // ✅ Load all invoices
        private void LoadInvoices()
        {
            lstInvoices.Items.Clear();

            var sales = _context.Sales.OrderByDescending(s => s.Date).ToList();
            foreach (var sale in sales)
            {
                lstInvoices.Items.Add(new InvoiceDisplayItem
                {
                    Id = sale.Id,
                    Display = $"Invoice #{sale.InvoiceCode} | Date: {sale.Date.ToShortDateString()} | Total: {sale.TotalAmount} EGP"
                });
            }
        }

        // ✅ Search by Invoice Code
        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            string searchCode = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchCode))
            {
                LoadInvoices();
                return;
            }

            var sale = _context.Sales
                .Include(s => s.Items)
                .FirstOrDefault(s => s.InvoiceCode == searchCode);

            lstInvoices.Items.Clear();

            if (sale != null)
            {
                lstInvoices.Items.Add(new InvoiceDisplayItem
                {
                    Id = sale.Id,
                    Display = $"Invoice #{sale.InvoiceCode} | Date: {sale.Date.ToShortDateString()} | Total: {sale.TotalAmount} EGP"
                });

                var view = new InvoiceViewForm(sale);
                view.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invoice not found!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ✅ View invoice details on double click
        private void LstInvoices_DoubleClick(object? sender, EventArgs e)
        {
            if (lstInvoices.SelectedIndex == -1) return;

            var selectedItem = lstInvoices.SelectedItem as InvoiceDisplayItem;
            if (selectedItem == null) return;

            var selectedSale = _context.Sales
                .Include(s => s.Items)
                .FirstOrDefault(s => s.Id == selectedItem.Id);

            if (selectedSale != null)
            {
                var view = new InvoiceViewForm(selectedSale);
                view.ShowDialog();
            }
        }

        // ✅ Wrapper class to store sale ID and text
        private class InvoiceDisplayItem
        {
            public int Id { get; set; }
            public string Display { get; set; } = "";

            public override string ToString()
            {
                return Display;
            }
        }
    }
}

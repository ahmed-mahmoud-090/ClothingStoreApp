using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClothingStoreApp.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ClothingStoreApp
{
    public partial class InvoiceViewForm : MaterialForm
    {
        private Sale _sale; // ✅ هنا تعريف المتغير
        private ListBox? lstDetails;
        private Label? lblTotal;
        private Button? btnPrint;
        private Button? btnBack;

        public InvoiceViewForm(Sale sale)
        {
            _sale = sale; // ✅ تخزين الفاتورة

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Teal600, Primary.Teal700,
                Primary.Teal200, Accent.Teal200,
                TextShade.WHITE
            );

            this.Text = $"Invoice Details                                             {sale.InvoiceCode}";
            this.Size = new Size(500, 500);

            lstDetails = new ListBox
            {
                Location = new Point(30, 100),
                Size = new Size(350, 230)
            };

            foreach (var item in sale.Items)
            {
                lstDetails.Items.Add($"{item.ProductName} x {item.Quantity} = {item.Quantity * item.UnitPrice} EGP");
            }

            lblTotal = new Label
            {
                Text = $"Total: {sale.TotalAmount} EGP",
                Location = new Point(30, 350),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true
            };

            btnPrint = new Button
            {
                Text = "Print Invoice",
                Location = new Point(30, 400),
                Size = new Size(100, 30)
            };
            btnPrint.Click += BtnPrint_Click;

            btnBack = new Button
            {
                Text = "Back",
                Location = new Point(150, 400)
            };
            btnBack.Click += (s, e) => this.Close();

            this.Controls.Add(lstDetails);
            this.Controls.Add(lblTotal);
            this.Controls.Add(btnPrint);
            this.Controls.Add(btnBack);
        }

        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    Title = "Save Invoice as PDF",
                    FileName = $"Invoice_{_sale.InvoiceCode}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = $"Invoice {_sale.InvoiceCode}";

                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
                    XFont boldFont = new XFont("Verdana", 14, XFontStyle.Bold);
                    XFont headerFont = new XFont("Verdana", 16, XFontStyle.Bold);

                    int y = 40;

                    // Draw logo at top left
                    string logoPath = Path.Combine(Application.StartupPath, "logo.jpeg");
                    if (File.Exists(logoPath))
                    {
                        XImage logo = XImage.FromFile(logoPath);
                        gfx.DrawImage(logo, 20, y, 100, 50);
                    }

                    // Header
                    gfx.DrawString("INVOICE", headerFont, XBrushes.Black, new XRect(200, y, page.Width, 50), XStringFormats.TopLeft);
                    y += 70;

                    gfx.DrawString($"Invoice Code: {_sale.InvoiceCode}", boldFont, XBrushes.Black, new XRect(20, y, page.Width, page.Height), XStringFormats.TopLeft);
                    y += 30;
                    gfx.DrawString($"Date: {_sale.Date.ToShortDateString()}", font, XBrushes.Black, new XRect(20, y, page.Width, page.Height), XStringFormats.TopLeft);
                    y += 30;
                    gfx.DrawString($"Total Amount: {_sale.TotalAmount} EGP", font, XBrushes.Black, new XRect(20, y, page.Width, page.Height), XStringFormats.TopLeft);
                    y += 40;
                    gfx.DrawString("Items:", boldFont, XBrushes.Black, new XRect(20, y, page.Width, page.Height), XStringFormats.TopLeft);
                    y += 25;

                    foreach (var item in _sale.Items)
                    {
                        gfx.DrawString($"{item.ProductName} - Qty: {item.Quantity} - Price: {item.UnitPrice} EGP", font, XBrushes.Black, new XRect(20, y, page.Width, page.Height), XStringFormats.TopLeft);
                        y += 25;
                    }

                    // Signature
                    y += 50;
                    gfx.DrawString("Authorized Signature:", font, XBrushes.Black, new XRect(20, y, page.Width, page.Height), XStringFormats.TopLeft);

                    string signaturePath = Path.Combine(Application.StartupPath, "signature.png");
                    if (File.Exists(signaturePath))
                    {
                        XImage signature = XImage.FromFile(signaturePath);
                        gfx.DrawImage(signature, 180, y - 10, 100, 50);
                    }

                    document.Save(saveFileDialog.FileName);
                    MessageBox.Show("Invoice saved as PDF successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}");
            }
        }
    }
}

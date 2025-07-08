using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStoreApp.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public string? InvoiceCode { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);

        public List<SaleItem> Items { get; set; } = new List<SaleItem>();

    }
}

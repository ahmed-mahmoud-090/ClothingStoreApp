using System;

namespace ClothingStoreApp.Models
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public int SaleId { get; set; }

        public string ProductName { get; set; } = string.Empty; // ✅ تجنب null
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Sale Sale { get; set; } = null!; // ✅ نضمن عدم null مع null-forgiving
    }
}

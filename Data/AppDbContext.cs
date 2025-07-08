using ClothingStoreApp.Models;
using System.Configuration;
using System.Data.Entity;
namespace ClothingStoreApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=ClothingStoreConnection") { 
        
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Models.Product> Products { get; set; } = null!;
        public DbSet<Models.Sale> Sales { get; set; } = null!;
        public DbSet<Models.SaleItem> SaleItems { get; set; } = null!;

        

    }
}


using Microsoft.EntityFrameworkCore;
using Session07.Areas.Admin.Models.DataModels;

namespace WebFoofMart.Models.DataModels
{
    public class FoodDbContext:DbContext
    {
        public FoodDbContext()
        {

        }
        public FoodDbContext(DbContextOptions<FoodDbContext> options):base (options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountAdmin> AcountAdmins { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    }

}

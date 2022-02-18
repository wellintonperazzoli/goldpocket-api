using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoldPocket.Data
{
    public class GoldPocketContext : IdentityDbContext
    {
        public GoldPocketContext(DbContextOptions<GoldPocketContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<appUser> appUser { get; set; }
        public DbSet<LocationDb> Location { get; set; }
        public DbSet<CategoryDb> Category { get; set; }
        public DbSet<ItemDb> Item { get; set; }
        public DbSet<expenseItemDb> expenseItem { get; set; }
        public DbSet<ExpenseDb> Expense { get; set; }
    }
}

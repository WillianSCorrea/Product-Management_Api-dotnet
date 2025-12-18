using Microsoft.EntityFrameworkCore;
using Product_Management.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace Product_Management.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
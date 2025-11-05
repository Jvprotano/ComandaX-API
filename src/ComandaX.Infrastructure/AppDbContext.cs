using Microsoft.EntityFrameworkCore;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<CustomerTab> CustomerTabs => Set<CustomerTab>();
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Order>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Table>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}

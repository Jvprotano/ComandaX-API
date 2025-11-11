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
        modelBuilder.Entity<Order>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<CustomerTab>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Product>().HasQueryFilter(p => p.DeletedAt == null);
        modelBuilder.Entity<Table>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<ProductCategory>().HasQueryFilter(pc => pc.DeletedAt == null);
        modelBuilder.Entity<Order>().HasQueryFilter(o => o.DeletedAt == null);
        modelBuilder.Entity<CustomerTab>().HasQueryFilter(ct => ct.DeletedAt == null);
        modelBuilder.Entity<OrderProduct>().HasQueryFilter(op => op.DeletedAt == null);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);

        base.OnModelCreating(modelBuilder);
    }
}

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
}

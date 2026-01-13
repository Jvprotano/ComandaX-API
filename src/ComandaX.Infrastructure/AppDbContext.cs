using Microsoft.EntityFrameworkCore;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Common;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure;

public class AppDbContext : DbContext
{
    private readonly ITenantService? _tenantService;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<CustomerTab> CustomerTabs => Set<CustomerTab>();
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auto-increment Code properties
        modelBuilder.Entity<Order>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<CustomerTab>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();

        // Configure Tenant entity (no tenant filter - tenants are root entities)
        modelBuilder.Entity<Tenant>().HasQueryFilter(t => t.DeletedAt == null);

        // Configure Subscription entity (no tenant filter - subscriptions are queried by TenantId explicitly)
        modelBuilder.Entity<Subscription>()
            .HasQueryFilter(s => s.DeletedAt == null);
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Tenant)
            .WithOne()
            .HasForeignKey<Subscription>(s => s.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Subscription>()
            .HasIndex(s => s.TenantId)
            .IsUnique();

        // Configure User-Tenant relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure TenantId indexes for better query performance
        modelBuilder.Entity<Order>().HasIndex(o => o.TenantId);
        modelBuilder.Entity<CustomerTab>().HasIndex(ct => ct.TenantId);
        modelBuilder.Entity<Table>().HasIndex(t => t.TenantId);
        modelBuilder.Entity<Product>().HasIndex(p => p.TenantId);
        modelBuilder.Entity<ProductCategory>().HasIndex(pc => pc.TenantId);
        modelBuilder.Entity<OrderProduct>().HasIndex(op => op.TenantId);
        modelBuilder.Entity<User>().HasIndex(u => u.TenantId);

        // Soft delete and tenant query filters
        // Each entity is filtered by both DeletedAt and TenantId
        var tenantId = _tenantService?.GetCurrentTenantId();

        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => p.DeletedAt == null && (tenantId == null || p.TenantId == tenantId));
        modelBuilder.Entity<Table>()
            .HasQueryFilter(t => t.DeletedAt == null && (tenantId == null || t.TenantId == tenantId));
        modelBuilder.Entity<ProductCategory>()
            .HasQueryFilter(pc => pc.DeletedAt == null && (tenantId == null || pc.TenantId == tenantId));
        modelBuilder.Entity<Order>()
            .HasQueryFilter(o => o.DeletedAt == null && (tenantId == null || o.TenantId == tenantId));
        modelBuilder.Entity<CustomerTab>()
            .HasQueryFilter(ct => ct.DeletedAt == null && (tenantId == null || ct.TenantId == tenantId));
        modelBuilder.Entity<OrderProduct>()
            .HasQueryFilter(op => op.DeletedAt == null && (tenantId == null || op.TenantId == tenantId));
        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.DeletedAt == null && (tenantId == null || u.TenantId == tenantId));

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        SetTenantIdForNewEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTenantIdForNewEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetTenantIdForNewEntities()
    {
        var tenantId = _tenantService?.GetCurrentTenantId();
        if (tenantId == null || tenantId == Guid.Empty)
            return;

        var entries = ChangeTracker.Entries<ITenantEntity>()
            .Where(e => e.State == EntityState.Added && e.Entity.TenantId == Guid.Empty);

        foreach (var entry in entries)
        {
            entry.Entity.SetTenantId(tenantId.Value);
        }
    }
}

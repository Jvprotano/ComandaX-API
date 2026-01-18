using Microsoft.EntityFrameworkCore;
using Bogus;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure;

public class SeedData
{
    /// <summary>
    /// Default tenant ID for development and seeding purposes.
    /// In production, this should be replaced with proper tenant management.
    /// </summary>
    public static readonly Guid DefaultTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public static async Task SeedDefaultTenantAsync(AppDbContext context)
    {
        if (!await context.Tenants.AnyAsync(t => t.Id == DefaultTenantId))
        {
            var defaultTenant = new Tenant("Default Tenant", "Default tenant for development");
            // Use reflection to set the Id since it's internal set
            typeof(Tenant).GetProperty("Id")!.SetValue(defaultTenant, DefaultTenantId);
            context.Tenants.Add(defaultTenant);
            await context.SaveChangesAsync();
        }

        // Ensure default tenant has a subscription (trial)
        if (!await context.Subscriptions.AnyAsync(s => s.TenantId == DefaultTenantId))
        {
            var subscription = new Subscription(DefaultTenantId);
            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedAdminsAsync(AppDbContext context)
    {
        // Ensure default tenant exists first
        await SeedDefaultTenantAsync(context);

        var admins = new[]
        {
            "jvprotano@gmail.com",
            "vhprotano@gmail.com"
        };

        foreach (var email in admins)
        {
            if (!await context.Users.IgnoreQueryFilters().AnyAsync(u => u.Email == email))
            {
                var user = new User(email, "Admin", DefaultTenantId);
                context.Users.Add(user);
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task GenerateDevTestData(AppDbContext context)
    {
        // Ensure default tenant exists first
        await SeedDefaultTenantAsync(context);

        if (await context.ProductCategories.IgnoreQueryFilters().AnyAsync())
            return;

        var productCategories = new Faker<ProductCategory>()
            .CustomInstantiator(f => new ProductCategory(f.Commerce.Department()))
            .FinishWith((f, pc) => pc.SetTenantId(DefaultTenantId))
            .Generate(3);
        await context.ProductCategories.AddRangeAsync(productCategories);
        await context.SaveChangesAsync();

        var products = new Faker<Product>()
            .CustomInstantiator(f => new Product(f.Commerce.ProductName(), f.Random.Decimal(1, 100), false, f.Random.ListItem(productCategories).Id))
            .FinishWith((f, p) => p.SetTenantId(DefaultTenantId))
            .Generate(5);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        var tables = new Faker<Table>()
            .CustomInstantiator(f =>
            {
                var table = new Table();
                table.SetTenantId(DefaultTenantId);
                return table;
            })
            .Generate(5);
        await context.Tables.AddRangeAsync(tables);
        await context.SaveChangesAsync();

        var customerTabs = new Faker<CustomerTab>()
            .CustomInstantiator(f =>
            {
                var tab = new CustomerTab(f.Name.FullName(), f.Random.ListItem(tables).Id);
                tab.SetTenantId(DefaultTenantId);
                return tab;
            })
            .Generate(5);
        await context.CustomerTabs.AddRangeAsync(customerTabs);
        await context.SaveChangesAsync();

        var orders = new Faker<Order>()
            .CustomInstantiator(f =>
            {
                var order = new Order(f.Random.ListItem(customerTabs).Id);
                order.SetTenantId(DefaultTenantId);
                return order;
            })
            .Generate(5);
        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();

        var orderProducts = new Faker<OrderProduct>()
            .CustomInstantiator(f =>
            {
                var orderProduct = new OrderProduct(f.Random.ListItem(orders).Id, f.Random.ListItem(products), f.Random.Decimal(1, 5));
                orderProduct.SetTenantId(DefaultTenantId);
                return orderProduct;
            })
            .Generate(10);
        await context.OrderProducts.AddRangeAsync(orderProducts);
        await context.SaveChangesAsync();
    }
}

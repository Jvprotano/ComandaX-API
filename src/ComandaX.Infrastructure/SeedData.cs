using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bogus;
using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure;

public class SeedData
{
    public static async Task SeedAdminsAsync(AppDbContext context)
    {
        var admins = new[]
        {
            "jvprotano@gmail.com",
            "vhprotano@gmail.com",
            "protanosoftware@gmail.com"
        };

        foreach (var email in admins)
        {
            if (!context.Users.Any(u => u.Email == email))
            {
                context.Users.Add(new User(email, "Admin"));
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task GenerateDevTestData(AppDbContext context)
    {
        if (await context.ProductCategories.AnyAsync())
            return;

        var productCategories = new Faker<ProductCategory>()
            .CustomInstantiator(f => new ProductCategory(f.Commerce.Department()))
            .Generate(10);
        await context.ProductCategories.AddRangeAsync(productCategories);
        await context.SaveChangesAsync();

        var products = new Faker<Product>()
            .CustomInstantiator(f => new Product(f.Commerce.ProductName(), f.Random.Decimal(1, 100), f.Random.ListItem(productCategories).Id))
            .RuleFor(p => p.NeedPreparation, f => f.Random.Bool())
            .Generate(50);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        var tables = new Faker<Table>()
            .CustomInstantiator(f => new Table())
            .Generate(20);
        await context.Tables.AddRangeAsync(tables);
        await context.SaveChangesAsync();

        var customerTabs = new Faker<CustomerTab>()
            .CustomInstantiator(f => new CustomerTab(f.Name.FullName(), f.Random.ListItem(tables).Id))
            .Generate(30);
        await context.CustomerTabs.AddRangeAsync(customerTabs);
        await context.SaveChangesAsync();

        var orders = new Faker<Order>()
            .CustomInstantiator(f => new Order(f.Random.ListItem(customerTabs).Id))
            .Generate(100);
        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();

        var orderProducts = new Faker<OrderProduct>()
            .CustomInstantiator(f => new OrderProduct(f.Random.ListItem(orders).Id, f.Random.ListItem(products).Id, f.Random.Int(1, 5)))
            .Generate(200);
        await context.OrderProducts.AddRangeAsync(orderProducts);
        await context.SaveChangesAsync();
    }

}

using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure.Persistence;

public class DbFake
{
    private static Dictionary<string, object> Database { get; } = [];

    static DbFake()
    {
        if (!Database.ContainsKey("Products"))
        {
            Database["Products"] = new List<Product>();
        }

        AddProduct(new("Coca-Cola 600ml", 10.0m));
        AddProduct(new("Pastel de frango", 20.0m));
        AddProduct(new("Picolé de fruta", 30.0m));
    }

    public static IList<Product> GetProducts()
    {
        return (IList<Product>)Database["Products"];
    }
    public static Product AddProduct(Product product)
    {
        var products = GetProducts();

        var maxCode = products.Count == 0 ? 0 : products.Max(p => p.Code);

        product.SetNextCode(maxCode);

        products.Add(product);

        Database["Products"] = products;

        return product;
    }
}

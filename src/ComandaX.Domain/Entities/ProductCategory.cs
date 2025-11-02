namespace ComandaX.Domain.Entities;

public class ProductCategory : BaseEntity
{
    public ProductCategory(string name, string? icon = null)
    {
        Name = name;
        Icon = icon;
    }

    public string Name { get; private set; }
    public string? Icon { get; private set; }

    public IList<Product>? Products { get; set; }
}

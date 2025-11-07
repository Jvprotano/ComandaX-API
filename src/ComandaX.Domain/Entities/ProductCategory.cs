namespace ComandaX.Domain.Entities;

public class ProductCategory(string name, string? icon = null) : BaseEntity
{
    public string Name { get; private set; } = name;
    public string? Icon { get; private set; } = icon;
    public IList<Product>? Products { get; set; }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
    }

    public void UpdateIcon(string? icon)
    {
        Icon = icon;
    }
}

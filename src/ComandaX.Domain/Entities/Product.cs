namespace ComandaX.Domain.Entities;

public sealed class Product(string name, decimal price, Guid? productCategoryId = null) : BaseEntity
{
    public string Name { get; private set; } = name;
    public decimal Price { get; private set; } = price;
    public int Code { get; private set; }
    public bool NeedPreparation { get; private set; } = false;
    public Guid? ProductCategoryId { get; private set; } = productCategoryId;
    public ProductCategory? ProductCategory { get; private set; }

    public void SetNeedPreparation(bool needPreparation)
    {
        NeedPreparation = needPreparation;
        EntityUpdated();
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name.Trim();
        EntityUpdated();
    }

    public void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));

        Price = price;
        EntityUpdated();
    }

    public void SetProductCategory(Guid? productCategoryId)
    {
        ProductCategoryId = productCategoryId;
        EntityUpdated();
    }
}

namespace ComandaX.Domain.Entities;

public sealed class Product : BaseEntity
{
    public Product(string name, decimal price, Guid? productCategoryId = null)
    {
        Name = name;
        Price = price;
        ProductCategoryId = productCategoryId;
    }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Code { get; private set; }
    public bool NeedPreparation { get; private set; } = false;
    public Guid? ProductCategoryId { get; private set; }
    public ProductCategory? ProductCategory { get; private set; }

    public void SetNeedPreparation(bool needPreparation)
    {
        NeedPreparation = needPreparation;
        EntityUpdated();
    }

    public void SetName(string name)
    {
        Name = name.Trim();
        EntityUpdated();
    }

    public void SetPrice(decimal price)
    {
        Price = price;
        EntityUpdated();
    }

    public void SetProductCategory(Guid? productCategoryId)
    {
        ProductCategoryId = productCategoryId;
        EntityUpdated();
    }
}

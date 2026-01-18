using ComandaX.Domain.Common;

namespace ComandaX.Domain.Entities;

public sealed class Product : BaseEntity, ITenantEntity
{
    public Product()
    {
        Name = string.Empty;
        Price = decimal.MinValue;
    }
    public Product(string name, decimal price, bool pricePerKg, Guid? productCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));

        Name = name.Trim();
        Price = price;
        IsPricePerKg = pricePerKg;
        ProductCategoryId = productCategoryId;
    }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Code { get; private set; }
    public bool NeedPreparation { get; private set; } = false;
    public Guid? ProductCategoryId { get; private set; }
    public ProductCategory? ProductCategory { get; private set; }
    public bool IsPricePerKg { get; private set; } = false;

    public void SetTenantId(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));
        TenantId = tenantId;
    }

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

    public void SetIsPricePerKg(bool isPricePerKg)
    {
        IsPricePerKg = isPricePerKg;
        EntityUpdated();
    }
}

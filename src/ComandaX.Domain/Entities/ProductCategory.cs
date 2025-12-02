using ComandaX.Domain.Common;

namespace ComandaX.Domain.Entities;

public class ProductCategory(string name, string? icon = null) : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = name;
    public string? Icon { get; private set; } = icon;
    public IList<Product>? Products { get; set; }

    public void SetTenantId(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));
        TenantId = tenantId;
    }

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

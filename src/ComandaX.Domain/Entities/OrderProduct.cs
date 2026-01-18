using ComandaX.Domain.Common;
using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public class OrderProduct : BaseEntity, ITenantEntity
{
    public OrderProduct()
    {

    }

    public OrderProduct(Guid orderId, Product product, decimal quantity)
    {
        OrderId = orderId;
        ProductId = product.Id;

        if (!product.IsPricePerKg)
            Quantity = (int)quantity;
        else
            Quantity = quantity;

        TotalPrice = quantity * product.Price;
    }

    public Guid TenantId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal TotalPrice { get; private set; }
    public decimal Quantity { get; private set; }
    public OrderProductEnum? Status { get; private set; }

    public Order? Order { get; set; }
    public Product? Product { get; set; }

    public void SetTenantId(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));
        TenantId = tenantId;
    }
}

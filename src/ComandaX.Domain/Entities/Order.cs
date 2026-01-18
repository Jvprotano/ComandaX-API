using ComandaX.Domain.Common;
using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Order : BaseEntity, ITenantEntity
{
    public Order()
    {

    }

    public Order(Guid customerTabId)
    {
        SetCustomerTab(customerTabId);
    }

    public Guid TenantId { get; private set; }
    public int Code { get; private set; }
    public OrderStatusEnum Status { get; private set; } = OrderStatusEnum.Closed;
    public Guid? CustomerTabId { get; private set; }
    public CustomerTab? CustomerTab { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; private set; } = [];

    public void SetTenantId(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty", nameof(tenantId));
        TenantId = tenantId;
    }

    public void AddProduct(Product product, decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (product.Price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(product.Price));

        if (product.Id == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty", nameof(product.Id));

        OrderProducts.Add(new(Id, product, quantity));
    }

    public void StartPreparation()
    {
        Status = OrderStatusEnum.InPreparation;
        EntityUpdated();
    }

    public void SetCustomerTab(Guid customerTabId)
    {
        if (customerTabId == Guid.Empty)
            throw new ArgumentException("Customer tab ID cannot be empty", nameof(customerTabId));

        Status = OrderStatusEnum.Created;
        CustomerTabId = customerTabId;
    }

    public void CloseOrder()
    {
        Status = OrderStatusEnum.Closed;
        EntityUpdated();
    }
}

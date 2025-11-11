using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Order : BaseEntity
{
    public Order()
    {

    }

    public Order(Guid customerTabId)
    {
        SetCustomerTab(customerTabId);
    }

    public int Code { get; private set; }
    public OrderStatusEnum Status { get; private set; } = OrderStatusEnum.Closed;
    public Guid? CustomerTabId { get; private set; }
    public CustomerTab? CustomerTab { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; private set; } = [];

    public void AddProduct(Guid productId, int quantity, decimal price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));

        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty", nameof(productId));

        OrderProducts.Add(new(Id, productId, quantity, price));
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

using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Order : BaseEntity
{
    public Order()
    {

    }

    public Order(Guid customerTabId)
    {
        CustomerTabId = customerTabId;
    }

    public int Code { get; private set; }
    public OrderStatusEnum Status { get; private set; } = OrderStatusEnum.Created;
    public Guid? CustomerTabId { get; private set; }
    public CustomerTab? CustomerTab { get; set; }
    public IList<OrderProduct> OrderProducts { get; private set; } = [];

    public void AddProduct(Guid productId, int quantity, decimal price)
    {
        var orderProduct = new OrderProduct(Id, productId, quantity, price);
        OrderProducts.Add(orderProduct);
        EntityUpdated();
    }

    public void StartPreparation()
    {
        Status = OrderStatusEnum.InPreparation;
        EntityUpdated();
    }

    public void SetCustomerTab(Guid customerTabId)
    {
        CustomerTabId = customerTabId;
        EntityUpdated();

    }

    public void CloseOrder()
    {
        Status = OrderStatusEnum.Closed;
        EntityUpdated();
    }
}

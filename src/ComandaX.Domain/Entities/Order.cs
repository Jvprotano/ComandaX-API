using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Order
{
    public Order(Guid tabId)
    {
        Id = Guid.NewGuid();
        TabId = tabId;
        Status = OrderStatusEnum.Created;
    }

    public Guid Id { get; }
    public OrderStatusEnum Status { get; private set; }
    public IList<OrderProduct> OrderProducts { get; set; } = [];
    public Guid TabId { get; set; }
    public Tab? Tab { get; set; }

    public void StartPreparation()
    {
        Status = OrderStatusEnum.InPreparation;
    }

    public void CloseOrder()
    {
        Status = OrderStatusEnum.Closed;
    }
}

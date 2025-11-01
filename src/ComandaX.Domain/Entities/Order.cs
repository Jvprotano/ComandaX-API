using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public sealed class Order  : BaseEntity
{
    public Order(Guid tabId)
    {
        TabId = tabId;
        Status = OrderStatusEnum.Created;
    }

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

using ComandaX.Domain.Enums;

namespace ComandaX.Domain.Entities;

public class OrderProduct : BaseEntity
{
    public OrderProduct()
    {

    }

    public OrderProduct(Guid orderId, Guid productId, int quantity, decimal productPrice)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;

        TotalPrice = quantity * productPrice;
    }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal TotalPrice { get; private set; }
    public int Quantity { get; private set; }
    public OrderProductEnum? Status { get; private set; }

    public Order? Order { get; set; }
    public Product? Product { get; set; }
}

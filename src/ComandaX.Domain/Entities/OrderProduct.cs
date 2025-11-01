namespace ComandaX.Domain.Entities;

public class OrderProduct : BaseEntity
{
    public OrderProduct(Guid orderId, Guid productId, int quantity)
    {
        this.OrderId = orderId;
        this.ProductId = productId;
        this.Quantity = quantity;

        this.TotalPrice = quantity * (Product?.Price ?? 0);
    }
    public Guid OrderId { get; private set; }
    public Order? Order { get; set; }
    public Guid ProductId { get; private set; }
    public Product? Product { get; set; }
    public decimal TotalPrice { get; private set; }
    public int Quantity { get; private set; }
}

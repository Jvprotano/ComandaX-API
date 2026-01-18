namespace ComandaX.Application.DTOs;

public sealed record OrderProductDto
{
    public OrderProductDto() { }
    public OrderProductDto(Guid productId, Guid orderId, decimal quantity, decimal totalPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        TotalPrice = totalPrice;
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

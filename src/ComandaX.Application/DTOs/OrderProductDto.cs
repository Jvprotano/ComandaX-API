namespace ComandaX.Application.DTOs;

public sealed record OrderProductDto
{
    public OrderProductDto()
    {

    }
    public OrderProductDto(Guid productId, int quantity, decimal totalPrice)
    {
        this.ProductId = productId;
        this.Quantity = quantity;
        this.TotalPrice = totalPrice;
    }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public ProductDto? Product { get; set; }
}

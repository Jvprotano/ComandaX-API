namespace ComandaX.Application.DTOs;

public sealed record OrderProductDto
{
    public OrderProductDto()
    {

    }
    public OrderProductDto(Guid productId, int quantity, decimal totalPrice, ProductDto? product = null)
    {
        ProductId = productId;
        Quantity = quantity;
        TotalPrice = totalPrice;
        Product = product;
    }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public ProductDto? Product { get; set; }
}

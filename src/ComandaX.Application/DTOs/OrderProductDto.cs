namespace ComandaX.Application.DTOs;

public sealed record OrderProductDto(
    Guid ProductId,
    int Quantity,
    decimal TotalPrice,
    ProductDto? Product = null
);

namespace ComandaX.Application.DTOs;

public sealed record OrderProductDto(
    string ProductName,
    string UnitPrice,
    int Quantity,
    string TotalPrice
);

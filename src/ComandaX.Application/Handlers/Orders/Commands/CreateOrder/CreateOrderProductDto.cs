namespace ComandaX.Application.Handlers.Orders.Commands.CreateOrder;

public sealed record CreateOrderProductDto(
    Guid ProductId,
    decimal Quantity
);
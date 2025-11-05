using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid? CustomerTabId,
    IList<CreateOrderProductDto> Products) : IRequest<OrderDto>;
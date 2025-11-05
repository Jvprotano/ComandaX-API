using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<OrderDto>
{
    public Guid? CustomerTabId { get; set; }
    public List<Guid> ProductIds { get; set; } = [];
}

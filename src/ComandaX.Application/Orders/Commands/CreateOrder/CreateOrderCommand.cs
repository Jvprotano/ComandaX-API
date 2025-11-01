using MediatR;

namespace ComandaX.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<Guid>
{
    public Guid TabId { get; set; }
    public List<Guid> ProductIds { get; set; } = [];
}

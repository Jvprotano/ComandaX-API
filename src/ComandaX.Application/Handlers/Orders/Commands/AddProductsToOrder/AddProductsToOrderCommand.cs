using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.AddProductsToOrder;

public class AddProductsToOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public List<Guid> ProductIds { get; set; } = [];
}

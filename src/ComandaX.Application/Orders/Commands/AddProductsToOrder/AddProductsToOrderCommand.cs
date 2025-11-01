using MediatR;

namespace ComandaX.Application.Orders.Commands.AddProductsToOrder;

public class AddProductsToOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public List<Guid> ProductIds { get; set; } = [];
}

using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<Order>
{
    public Guid Id { get; set; }
}

using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<Order>
{
    public Guid Id { get; set; }
}

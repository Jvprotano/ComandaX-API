using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDto>
{
    public Guid Id { get; set; }
}

using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Handlers.Orders.Queries.GetOrderById;
using ComandaX.Application.Handlers.Orders.Queries.GetOrders;
using ComandaX.Domain.Enums;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class OrderQuery
{
    public async Task<IList<OrderDto>> GetOrders(
        [Service] IMediator mediator,
        OrderStatusEnum? status)
    {
        return await mediator.Send(new GetOrdersQuery(status));
    }

    public async Task<OrderDto> GetOrderById(Guid id, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetOrderByIdQuery { Id = id });
    }
}

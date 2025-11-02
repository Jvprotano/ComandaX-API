using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Handlers.Orders.Queries.GetOrderById;
using ComandaX.Application.Handlers.Orders.Queries.GetOrders;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class OrderQuery
{
    [UseProjection]
    public async Task<IQueryable<OrderDto>> GetOrders([Service] IMediator mediator)
    {
        var orders = await mediator.Send(new GetOrdersQuery());
        return orders.Select(order => order.AsDto());
    }

    public async Task<OrderDto> GetOrderById(Guid id, [Service] IMediator mediator)
    {
        var order = await mediator.Send(new GetOrderByIdQuery { Id = id });
        return order.AsDto();
    }
}

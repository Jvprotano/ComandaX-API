using ComandaX.Application.Orders.Queries.GetOrderById;
using ComandaX.Application.Orders.Queries.GetOrders;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class OrderQuery
{
    [UseProjection]
    public async Task<IQueryable<Order>> GetOrders([Service] IMediator mediator)
    {
        return await mediator.Send(new GetOrdersQuery());
    }

    public async Task<Order> GetOrderById(Guid id, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetOrderByIdQuery { Id = id });
    }
}

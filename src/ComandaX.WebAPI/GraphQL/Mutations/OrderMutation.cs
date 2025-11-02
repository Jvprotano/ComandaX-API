using MediatR;
using ComandaX.Application.Handlers.Orders.Commands.CreateOrder;
using ComandaX.Application.Handlers.Orders.Commands.CloseOrder;
using ComandaX.Application.Handlers.Orders.Commands.AddProductsToOrder;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class OrderMutation
{
    public async Task<Guid> CreateOrder(CreateOrderCommand command, [Service] IMediator mediator)
    {
        return await mediator.Send(command);
    }

    public async Task<bool> AddProductsToOrder(AddProductsToOrderCommand command, [Service] IMediator mediator)
    {
        await mediator.Send(command);
        return true;
    }

    public async Task<bool> CloseOrder(CloseOrderCommand command, [Service] IMediator mediator)
    {
        await mediator.Send(command);
        return true;
    }
}

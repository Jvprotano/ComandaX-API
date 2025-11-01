using ComandaX.Application.Orders.Commands.CreateOrder;
using ComandaX.Application.Orders.Commands.AddProductsToOrder;
using ComandaX.Application.Orders.Commands.CloseOrder;
using MediatR;

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

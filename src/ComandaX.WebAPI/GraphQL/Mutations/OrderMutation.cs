using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.Orders.Commands.AddProductsToOrder;
using ComandaX.Application.Handlers.Orders.Commands.CloseOrder;
using ComandaX.Application.Handlers.Orders.Commands.CreateOrder;
using ComandaX.Application.Handlers.Orders.Commands.DeleteOrder;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class OrderMutation
{
    public async Task<OrderDto> CreateOrderAsync(
        Guid? customerTabId,
        IList<CreateOrderProductInput> products,
        [Service] IMediator mediator)
    {
        var command = new CreateOrderCommand(
            customerTabId,
            [.. products.Select(p => new CreateOrderProductDto(p.ProductId, p.Quantity))]);

        return await mediator.Send(command);
    }

    public async Task<bool> AddProductsToOrderAsync(AddProductsToOrderCommand command, [Service] IMediator mediator)
    {
        await mediator.Send(command);
        return true;
    }

    public async Task<bool> CloseOrderAsync(CloseOrderCommand command, [Service] IMediator mediator)
    {
        await mediator.Send(command);
        return true;
    }

    public async Task<bool> DeleteOrderAsync(
        [Service] IMediator mediator,
        Guid id)
    {
        await mediator.Send(new DeleteOrderCommand(id));
        return true;
    }
}

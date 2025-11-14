using ComandaX.Application.Handlers.CustomerTabs.Commands.CloseCustomerTab;
using ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;
using ComandaX.Application.Handlers.CustomerTabs.Commands.DeleteCustomerTab;
using ComandaX.Application.Handlers.CustomerTabs.Commands.SendCustomerTabEmail;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class CustomerTabMutation
{
    public async Task<CustomerTab> CreateCustomerTab(CreateCustomerTabCommand input, [Service] IMediator mediator)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> CloseCustomerTab(CloseCustomerTabCommand command, [Service] IMediator mediator)
    {
        await mediator.Send(command);
        return true;
    }

    public async Task<bool> SendCustomerTabEmail(
    Guid customerTabId,
    string email,
    [Service] IMediator mediator)
    {
        return await mediator.Send(new SendCustomerTabEmailCommand(customerTabId, email));
    }

    public async Task<bool> DeleteCustomerTabAsync(
        [Service] IMediator mediator,
        Guid id)
    {
        await mediator.Send(new DeleteCustomerTabCommand(id));
        return true;
    }
}

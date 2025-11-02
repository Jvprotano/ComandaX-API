using ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;
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
}

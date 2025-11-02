using ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabById;
using ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabs;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class CustomerTabQuery
{
    [UseProjection]
    public async Task<IQueryable<CustomerTab>> GetCustomerTabs([Service] IMediator mediator)
    {
        return await mediator.Send(new GetCustomerTabsQuery());
    }

    public async Task<CustomerTab> GetCustomerTabById(Guid id, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetCustomerTabByIdQuery(id));
    }
}

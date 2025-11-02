using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabById;
using ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabs;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class CustomerTabQuery
{
    [UseProjection]
    public async Task<IQueryable<CustomerTabDto>> GetCustomerTabs([Service] IMediator mediator)
    {
        var customerTabs = await mediator.Send(new GetCustomerTabsQuery());

        return customerTabs.Select(tab => tab.AsDto());
    }

    public async Task<CustomerTabDto> GetCustomerTabById(Guid id, [Service] IMediator mediator)
    {
        var customerTab = await mediator.Send(new GetCustomerTabByIdQuery(id));
        return customerTab.AsDto();
    }
}

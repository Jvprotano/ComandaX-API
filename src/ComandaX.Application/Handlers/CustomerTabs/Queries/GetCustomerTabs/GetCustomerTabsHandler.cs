using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabs;

public class GetCustomerTabsHandler(ICustomerTabRepository _customerTabRepository) : IRequestHandler<GetCustomerTabsQuery, IQueryable<CustomerTab>>
{
    public async Task<IQueryable<CustomerTab>> Handle(GetCustomerTabsQuery request, CancellationToken cancellationToken)
    {
        var customertabs = await _customerTabRepository.GetAllAsync();
        return customertabs;
    }
}

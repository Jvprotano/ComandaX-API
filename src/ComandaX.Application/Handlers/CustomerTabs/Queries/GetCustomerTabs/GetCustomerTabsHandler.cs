using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Queries.GetCustomerTabs;

public class GetCustomerTabsHandler(ICustomerTabRepository _customerTabRepository) : IRequestHandler<GetCustomerTabsQuery, IQueryable<CustomerTabDto>>
{
    public async Task<IQueryable<CustomerTabDto>> Handle(GetCustomerTabsQuery request, CancellationToken cancellationToken)
    {
        var customertabs = await _customerTabRepository.GetAllAsync();

        if (request.Status.HasValue)
            customertabs = customertabs.Where(ct => ct.Status == request.Status.Value);

        return customertabs.Select(tab => tab.AsDto());
    }
}

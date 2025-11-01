using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.CustomerTabs.Queries.GetCustomerTabById;

public class GetCustomerTabByIdQueryHandler : IRequestHandler<GetCustomerTabByIdQuery, CustomerTab>
{
    private readonly ICustomerTabRepository _customerTabRepository;

    public GetCustomerTabByIdQueryHandler(ICustomerTabRepository customerTabRepository)
    {
        _customerTabRepository = customerTabRepository;
    }

    public async Task<CustomerTab> Handle(GetCustomerTabByIdQuery request, CancellationToken cancellationToken)
    {
        var customerTab = await _customerTabRepository.GetByIdAsync(request.Id)
         ?? throw new RecordNotFoundException("CustomerTab not found");

        return customerTab;
    }
}

using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;

public class CreateCustomerTabCommandHandler : IRequestHandler<CreateCustomerTabCommand, CustomerTab>
{
    private readonly ICustomerTabRepository _customerTabRepository;
    private readonly ITableRepository _tableRepository;

    public CreateCustomerTabCommandHandler(ICustomerTabRepository customerTabRepository, ITableRepository tableRepository)
    {
        _customerTabRepository = customerTabRepository;
        _tableRepository = tableRepository;
    }

    public async Task<CustomerTab> Handle(CreateCustomerTabCommand request, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.TableId);

        var customerTab = new CustomerTab(request.Name, table?.Id);

        return await _customerTabRepository.CreateAsync(customerTab);
    }
}

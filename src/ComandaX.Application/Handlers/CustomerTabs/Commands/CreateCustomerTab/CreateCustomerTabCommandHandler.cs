using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;

public class CreateCustomerTabCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCustomerTabCommand, CustomerTab>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CustomerTab> Handle(CreateCustomerTabCommand request, CancellationToken cancellationToken)
    {
        var customerTab = new CustomerTab(request.Name, request.TableId);

        if (request.TableId.HasValue)
        {
            var table = await _unitOfWork.Tables.GetByIdAsync(request.TableId.Value)
                ?? throw new RecordNotFoundException($"Table {request.TableId} not found");

            table.SetBusy();
            await _unitOfWork.Tables.UpdateAsync(table);
        }

        await _unitOfWork.CustomerTabs.CreateAsync(customerTab);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customerTab;
    }
}

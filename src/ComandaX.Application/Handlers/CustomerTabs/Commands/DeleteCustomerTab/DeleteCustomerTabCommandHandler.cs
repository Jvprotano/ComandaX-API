using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.DeleteCustomerTab;

public class DeleteCustomerTabCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCustomerTabCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCustomerTabCommand request, CancellationToken cancellationToken)
    {
        var customerTab = await _unitOfWork.CustomerTabs.GetByIdAsync(request.Id) 
            ?? throw new RecordNotFoundException(request.Id);

        customerTab.SoftDelete();

        await _unitOfWork.CustomerTabs.UpdateAsync(customerTab);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}


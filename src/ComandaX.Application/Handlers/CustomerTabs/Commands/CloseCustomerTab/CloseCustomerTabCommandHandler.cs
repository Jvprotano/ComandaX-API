using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Enums;
using MediatR;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CloseCustomerTab;

public class CloseCustomerTabCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CloseCustomerTabCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(CloseCustomerTabCommand request, CancellationToken cancellationToken)
    {
        // Get the customer tab
        var customerTab = await _unitOfWork.CustomerTabs.GetByIdAsync(request.CustomerTabId)
            ?? throw new RecordNotFoundException($"Customer tab with Id {request.CustomerTabId} not found.");

        // Close the customer tab
        customerTab.Close();
        await _unitOfWork.CustomerTabs.UpdateAsync(customerTab);

        // Get all orders associated with this customer tab
        var orders = await _unitOfWork.Orders.GetByCustomerTabIdAsync(request.CustomerTabId);

        // Close all orders that are not already closed
        foreach (var order in orders.Where(o => o.Status != OrderStatusEnum.Closed))
        {
            order.CloseOrder();
            await _unitOfWork.Orders.UpdateAsync(order);
        }

        // If the customer tab has a table, set it as available (free)
        if (customerTab.TableId.HasValue)
        {
            var table = await _unitOfWork.Tables.GetByIdAsync(customerTab.TableId.Value)
                ?? throw new RecordNotFoundException($"Table with Id {customerTab.TableId.Value} not found.");

            table.SetFree();
            await _unitOfWork.Tables.UpdateAsync(table);
        }

        // Save all changes in a single transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


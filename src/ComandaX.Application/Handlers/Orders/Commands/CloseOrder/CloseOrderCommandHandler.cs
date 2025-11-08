using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CloseOrder;

public class CloseOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CloseOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(CloseOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId)
            ?? throw new RecordNotFoundException($"Order with Id {request.OrderId} not found.");

        order.CloseOrder();

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

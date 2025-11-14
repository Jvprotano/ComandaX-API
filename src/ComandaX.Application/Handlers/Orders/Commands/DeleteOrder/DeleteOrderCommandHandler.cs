using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.Id) 
            ?? throw new RecordNotFoundException(request.Id);

        order.SoftDelete();

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}


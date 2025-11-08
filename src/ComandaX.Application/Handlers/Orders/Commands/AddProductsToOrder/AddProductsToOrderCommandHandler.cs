using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.AddProductsToOrder;

public class AddProductsToOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddProductsToOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(AddProductsToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId)
         ?? throw new RecordNotFoundException($"Order with Id {request.OrderId} not found.");

        foreach (var productId in request.ProductIds)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId)
                ?? throw new RecordNotFoundException($"Product with Id {productId} not found.");

            order.OrderProducts.Add(new Domain.Entities.OrderProduct(order.Id, product.Id, 1, product.Price));
        }

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

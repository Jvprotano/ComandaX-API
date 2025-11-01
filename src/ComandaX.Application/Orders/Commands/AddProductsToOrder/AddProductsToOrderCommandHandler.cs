using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Orders.Commands.AddProductsToOrder;

public class AddProductsToOrderCommandHandler : IRequestHandler<AddProductsToOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public AddProductsToOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(AddProductsToOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId)
         ?? throw new RecordNotFoundException($"Order with Id {request.OrderId} not found.");

        foreach (var productId in request.ProductIds)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new RecordNotFoundException($"Product with Id {productId} not found.");

            order.OrderProducts.Add(new Domain.Entities.OrderProduct(order.Id, product.Id, 1));
        }

        await _orderRepository.UpdateAsync(order);

        return Unit.Value;
    }
}

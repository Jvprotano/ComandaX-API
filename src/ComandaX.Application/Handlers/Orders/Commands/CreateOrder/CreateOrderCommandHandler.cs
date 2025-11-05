using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IProductRepository productRepository) : IRequestHandler<CreateOrderCommand, OrderDto>
{

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order();

        if (request.CustomerTabId.HasValue)
            order.SetCustomerTab(request.CustomerTabId.Value);

        if (request.Products is null || !request.Products.Any())
            throw new OrderWithoutProductsException();

        foreach (var item in request.Products)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId)
                ?? throw new RecordNotFoundException($"Product {item.ProductId} not found");

            order.AddProduct(item.ProductId, item.Quantity, product.Price);
        }

        await orderRepository.AddAsync(order);

        return order.AsDto();
    }
}

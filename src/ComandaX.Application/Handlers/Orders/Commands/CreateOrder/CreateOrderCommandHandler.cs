using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ITableRepository _tableRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, ITableRepository tableRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _tableRepository = tableRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.TabId)
            ?? throw new RecordNotFoundException($"Table {request.TabId} not found");

        var order = new Order(request.TabId);

        // TODO: Check if all products exist

        table.SetBusy();
        await _tableRepository.UpdateAsync(table);

        await _orderRepository.CreateAsync(order);

        return order.Id;
    }
}

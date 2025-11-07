using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CloseOrder;

public class CloseOrderCommandHandler(IOrderRepository orderRepository, ITableRepository tableRepository) : IRequestHandler<CloseOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ITableRepository _tableRepository = tableRepository;

    public async Task<Unit> Handle(CloseOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId)
            ?? throw new RecordNotFoundException($"Order with Id {request.OrderId} not found.");

        order.CloseOrder();

        await _orderRepository.UpdateAsync(order);

        return Unit.Value;
    }
}

using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order();

        if (request.CustomerTabId.HasValue)
            order.SetCustomerTab(request.CustomerTabId.Value);

        await _orderRepository.AddAsync(order);

        return order.AsDto();
    }
}

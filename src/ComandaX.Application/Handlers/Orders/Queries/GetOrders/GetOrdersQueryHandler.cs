using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrdersQuery, IList<OrderDto>>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<IList<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync();
        return [.. orders.Select(o => o.AsDto())];
    }
}

using ComandaX.Application.DTOs;
using ComandaX.Domain.Enums;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery(OrderStatusEnum? Status) : IRequest<IList<OrderDto>>;
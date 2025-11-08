using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery : IRequest<IList<Order>>;
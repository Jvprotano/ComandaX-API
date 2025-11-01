using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery : IRequest<IQueryable<Order>>;
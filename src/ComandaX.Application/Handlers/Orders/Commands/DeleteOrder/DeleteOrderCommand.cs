using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.DeleteOrder;

public sealed record DeleteOrderCommand(Guid Id) : IRequest;


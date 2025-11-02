using MediatR;

namespace ComandaX.Application.Handlers.Orders.Commands.CloseOrder;

public sealed record CloseOrderCommand(Guid OrderId) : IRequest<Unit>;

using MediatR;

namespace ComandaX.Application.Orders.Commands.CloseOrder;

public sealed record CloseOrderCommand(Guid OrderId) : IRequest<Unit>;

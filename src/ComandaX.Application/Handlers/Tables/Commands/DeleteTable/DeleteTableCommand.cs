using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.DeleteTable;

public sealed record DeleteTableCommand(Guid Id) : IRequest;
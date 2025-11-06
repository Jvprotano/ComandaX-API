using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.UpdateTable;

public sealed record UpdateTableCommand(Guid Id, int? Number) : IRequest<TableDto>;

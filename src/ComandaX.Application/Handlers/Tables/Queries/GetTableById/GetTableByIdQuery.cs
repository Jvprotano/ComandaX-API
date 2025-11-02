using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Queries.GetTableById;

public record GetTableByIdQuery(Guid Id) : IRequest<TableDto>;

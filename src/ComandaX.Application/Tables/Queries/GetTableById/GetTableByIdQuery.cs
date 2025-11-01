using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Tables.Queries.GetTableById;

public record GetTableByIdQuery(Guid Id) : IRequest<TableDto>;

using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Queries.GetTables;

public record GetTablesQuery() : IRequest<IQueryable<TableDto>>;

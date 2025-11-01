using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Tables.Queries.GetTables;

public record GetTablesQuery() : IRequest<IEnumerable<TableDto>>;

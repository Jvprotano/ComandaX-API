using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Queries.GetTables;

public class GetTablesQueryHandler(ITableRepository repository) : IRequestHandler<GetTablesQuery, IEnumerable<TableDto>>
{

    public async Task<IEnumerable<TableDto>> Handle(GetTablesQuery request, CancellationToken cancellationToken)
    {
        var tables = await repository.GetAllAsync();

        return tables.Select(t => t.AsDto());
    }
}

using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Queries.GetTables;

public class GetTablesQueryHandler : IRequestHandler<GetTablesQuery, IEnumerable<TableDto>>
{
    private readonly ITableRepository _repository;

    public GetTablesQueryHandler(ITableRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TableDto>> Handle(GetTablesQuery request, CancellationToken cancellationToken)
    {
        var tables = await _repository.GetAllAsync();

        return tables.Select(t => new TableDto(t.Id, t.Code, t.Status));
    }
}

using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.Tables.Queries.GetTableById;
using ComandaX.Application.Handlers.Tables.Queries.GetTables;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class TableQuery
{
    [UseSorting]
    public async Task<IEnumerable<TableDto>> GetTables([Service] IMediator mediator)
    {
        return await mediator.Send(new GetTablesQuery());
    }

    public async Task<TableDto> GetTableById(Guid id, [Service] IMediator mediator)
    {
        return await mediator.Send(new GetTableByIdQuery(id));
    }
}

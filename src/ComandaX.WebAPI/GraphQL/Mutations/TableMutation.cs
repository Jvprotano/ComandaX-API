using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.Tables.Commands.CreateTable;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class TableMutation
{
    public async Task<TableDto> CreateTable([Service] IMediator mediator)
    {
        return await mediator.Send(new CreateTableCommand());
    }
}

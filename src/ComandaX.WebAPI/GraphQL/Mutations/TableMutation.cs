using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.Tables.Commands.CreateTable;
using ComandaX.Application.Handlers.Tables.Commands.DeleteTable;
using ComandaX.Application.Handlers.Tables.Commands.UpdateTable;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class TableMutation
{
    public async Task<TableDto> CreateTable([Service] IMediator mediator, int? number)
    {
        return await mediator.Send(new CreateTableCommand(number));
    }

    public async Task<TableDto> UpdateTableAsync(
        [Service] ISender mediator,
        Guid id,
        int? number)
    {
        return await mediator.Send(new UpdateTableCommand(id, number));
    }

    public async Task<bool> DeleteTableAsync(
        [Service] ISender mediator,
        Guid id)
    {
        await mediator.Send(new DeleteTableCommand(id));
        return true;
    }
}

using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.CreateTable;

public class CreateTableCommandHandler(ITableRepository repository) : IRequestHandler<CreateTableCommand, TableDto>
{
    public async Task<TableDto> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        var table = new Table();

        if (request.Number != null)
            table.SetNumber(request.Number.Value);

        await repository.AddAsync(table);

        return table.AsDto();
    }
}

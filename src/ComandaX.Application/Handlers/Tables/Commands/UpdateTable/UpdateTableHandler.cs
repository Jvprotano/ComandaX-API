using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.UpdateTable;

public class UpdateTableHandler(ITableRepository tableRepository) : IRequestHandler<UpdateTableCommand, TableDto>
{
    public async Task<TableDto> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
    {
        var table = await tableRepository.GetByIdAsync(request.Id)
            ?? throw new RecordNotFoundException($"Table with Id {request.Id} not found");

        if (request.Number != null)
            table.SetNumber(request.Number.Value);

        await tableRepository.UpdateAsync(table);

        return table.AsDto();
    }
}

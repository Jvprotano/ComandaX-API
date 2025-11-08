using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.UpdateTable;

public class UpdateTableHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateTableCommand, TableDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TableDto> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
    {
        var table = await _unitOfWork.Tables.GetByIdAsync(request.Id)
            ?? throw new RecordNotFoundException($"Table with Id {request.Id} not found");

        table.SetNumber(request.Number);

        await _unitOfWork.Tables.UpdateAsync(table);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return table.AsDto();
    }
}

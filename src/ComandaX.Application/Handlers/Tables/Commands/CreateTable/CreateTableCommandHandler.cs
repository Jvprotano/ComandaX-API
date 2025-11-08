using ComandaX.Application.DTOs;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.CreateTable;

public class CreateTableCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTableCommand, TableDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TableDto> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        var table = new Table();

        if (request.Number != null)
            table.SetNumber(request.Number.Value);

        await _unitOfWork.Tables.AddAsync(table);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return table.AsDto();
    }
}

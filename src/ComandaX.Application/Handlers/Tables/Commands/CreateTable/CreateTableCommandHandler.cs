using ComandaX.Application.DTOs;
using ComandaX.Application.Interfaces;
using ComandaX.Domain.Entities;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.CreateTable;

public class CreateTableCommandHandler : IRequestHandler<CreateTableCommand, TableDto>
{
    private readonly ITableRepository _repository;

    public CreateTableCommandHandler(ITableRepository repository)
    {
        _repository = repository;
    }

    public async Task<TableDto> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        var maxNumber = await _repository.GetMaxCodeAsync();

        var table = new Table(maxNumber + 1);

        var newTable = await _repository.AddAsync(table);

        return new TableDto(newTable.Id, newTable.Code, newTable.Status);
    }
}

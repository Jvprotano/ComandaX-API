using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Tables.Queries.GetTableById;

public class GetTableByIdQueryHandler : IRequestHandler<GetTableByIdQuery, TableDto>
{
    private readonly ITableRepository _repository;

    public GetTableByIdQueryHandler(ITableRepository repository)
    {
        _repository = repository;
    }

    public async Task<TableDto> Handle(GetTableByIdQuery request, CancellationToken cancellationToken)
    {
        var table = await _repository.GetByIdAsync(request.Id) ?? throw new RecordNotFoundException($"Table with id {request.Id} not found");

        return new TableDto(table.Id, table.Code, table.Status);
    }
}

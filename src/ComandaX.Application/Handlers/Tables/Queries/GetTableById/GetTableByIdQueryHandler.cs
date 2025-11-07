using ComandaX.Application.DTOs;
using ComandaX.Application.Exceptions;
using ComandaX.Application.Extensions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Queries.GetTableById;

public class GetTableByIdQueryHandler(ITableRepository repository) : IRequestHandler<GetTableByIdQuery, TableDto>
{
    private readonly ITableRepository _repository = repository;

    public async Task<TableDto> Handle(GetTableByIdQuery request, CancellationToken cancellationToken)
    {
        var table = await _repository.GetByIdAsync(request.Id) ?? throw new RecordNotFoundException($"Table with id {request.Id} not found");

        return table.AsDto();
    }
}

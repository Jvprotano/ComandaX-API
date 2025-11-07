using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.DeleteTable;

public class DeleteTableCommandHandler(ITableRepository tableRepository) : IRequestHandler<DeleteTableCommand>
{
    private readonly ITableRepository _tableRepository = tableRepository;

    public async Task Handle(DeleteTableCommand request, CancellationToken cancellationToken)
    {
        var table = await _tableRepository.GetByIdAsync(request.Id) ?? throw new RecordNotFoundException(request.Id);

        table.SoftDelete();

        await _tableRepository.UpdateAsync(table);
    }
}
using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Tables.Commands.DeleteTable;

public class DeleteTableCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteTableCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteTableCommand request, CancellationToken cancellationToken)
    {
        var table = await _unitOfWork.Tables.GetByIdAsync(request.Id) ?? throw new RecordNotFoundException(request.Id);

        table.SoftDelete();

        await _unitOfWork.Tables.UpdateAsync(table);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
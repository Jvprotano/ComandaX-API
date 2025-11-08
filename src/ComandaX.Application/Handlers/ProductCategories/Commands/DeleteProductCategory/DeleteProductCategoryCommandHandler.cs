using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await _unitOfWork.ProductCategories.GetByIdAsync(request.Id) ?? throw new RecordNotFoundException(request.Id);

        productCategory.SoftDelete();
        await _unitOfWork.ProductCategories.UpdateAsync(productCategory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await _unitOfWork.ProductCategories.GetByIdAsync(request.Id);

        if (productCategory == null)
            throw new RecordNotFoundException("Product category not found");

        if (!string.IsNullOrEmpty(request.Name))
            productCategory.UpdateName(request.Name);

        if (request.Icon.HasValue && request.Icon.Value != string.Empty)
            productCategory.UpdateIcon(request.Icon.Value);

        await _unitOfWork.ProductCategories.UpdateAsync(productCategory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

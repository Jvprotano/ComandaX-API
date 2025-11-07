using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryCommandHandler(IProductCategoryRepository repository) : IRequestHandler<UpdateProductCategoryCommand, Unit>
{
    private readonly IProductCategoryRepository _repository = repository;

    public async Task<Unit> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await _repository.GetByIdAsync(request.Id);

        if (productCategory == null)
            throw new RecordNotFoundException("Product category not found");

        if (!string.IsNullOrEmpty(request.Name))
            productCategory.UpdateName(request.Name);

        if (request.Icon.HasValue && request.Icon.Value != string.Empty)
            productCategory.UpdateIcon(request.Icon.Value);

        await _repository.UpdateAsync(productCategory);

        return Unit.Value;
    }
}

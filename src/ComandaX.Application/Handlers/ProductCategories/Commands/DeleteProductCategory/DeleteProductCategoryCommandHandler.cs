using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand>
{
    private readonly IProductCategoryRepository _productCategoryRepository;

    public DeleteProductCategoryCommandHandler(IProductCategoryRepository productCategoryRepository)
    {
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id) ?? throw new RecordNotFoundException(request.Id);

        await _productCategoryRepository.DeleteAsync(productCategory);
    }
}
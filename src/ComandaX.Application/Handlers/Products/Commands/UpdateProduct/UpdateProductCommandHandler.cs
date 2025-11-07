using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IProductRepository repository) : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _repository = repository;

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id)
            ?? throw new RecordNotFoundException("Product not found");

        if (!string.IsNullOrWhiteSpace(request.Name))
            product.SetName(request.Name);

        if (request.Price != null)
            product.SetPrice(request.Price.Value);

        if (request.ProductCategoryId.HasValue)
            product.SetProductCategory(request.ProductCategoryId.Value);

        if (request.NeedPreparation != null)
            product.SetNeedPreparation(request.NeedPreparation.Value);

        await _repository.UpdateAsync(product);

        return Unit.Value;
    }
}

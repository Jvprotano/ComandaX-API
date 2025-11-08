using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id)
            ?? throw new RecordNotFoundException("Product not found");

        if (!string.IsNullOrWhiteSpace(request.Name))
            product.SetName(request.Name);

        if (request.Price != null)
            product.SetPrice(request.Price.Value);

        if (request.ProductCategoryId.HasValue)
            product.SetProductCategory(request.ProductCategoryId.Value);

        if (request.NeedPreparation != null)
            product.SetNeedPreparation(request.NeedPreparation.Value);

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

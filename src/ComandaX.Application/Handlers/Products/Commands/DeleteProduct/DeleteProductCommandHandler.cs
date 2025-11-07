using ComandaX.Application.Exceptions;
using ComandaX.Application.Interfaces;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id) ?? throw new RecordNotFoundException(request.Id);

        product.SoftDelete();

        await productRepository.UpdateAsync(product);
    }
}
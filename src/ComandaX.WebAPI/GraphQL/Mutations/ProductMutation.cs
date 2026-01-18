using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.Products.Commands.CreateProduct;
using ComandaX.Application.Handlers.Products.Commands.DeleteProduct;
using ComandaX.Application.Handlers.Products.Commands.UpdateProduct;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class ProductMutation
{
    public async Task<ProductDto> CreateProductAsync(
        [Service] ISender mediator,
        string name,
        decimal price,
        bool? needPreparation,
        Optional<Guid?> productCategoryId,
        bool isPricePerKg = false)
    {
        return await mediator.Send(
            new CreateProductCommand(
                name,
                price,
                productCategoryId,
                needPreparation ?? false, isPricePerKg));
    }

    public async Task<bool> UpdateProductAsync(
        [Service] ISender mediator,
        Guid id,
        string? name,
        decimal? price,
        bool? needPreparation,
        bool? isPricePerKg,
        Optional<Guid?> productCategoryId)
    {
        await mediator.Send(new UpdateProductCommand(id, name, price, needPreparation, isPricePerKg, productCategoryId));
        return true;
    }

    public async Task<bool> DeleteProductAsync(
        [Service] ISender mediator,
        Guid id)
    {
        await mediator.Send(new DeleteProductCommand(id));
        return true;
    }
}


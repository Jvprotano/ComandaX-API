using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.Products.Commands.CreateProduct;
using ComandaX.Application.Handlers.Products.Commands.UpdateProduct;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class ProductMutation
{
    public async Task<ProductDto> CreateProductAsync(
        [Service] ISender mediator,
        string name,
        decimal price)
    {
        return await mediator.Send(new CreateProductCommand(name, price));
    }

    public async Task<bool> UpdateProductAsync(
        [Service] ISender mediator,
        Guid id,
        string? name,
        decimal? price,
        bool? needPreparation,
        Optional<Guid?> productCategoryId)
    {
        await mediator.Send(new UpdateProductCommand(id, name, price, needPreparation, productCategoryId));
        return true;
    }
}


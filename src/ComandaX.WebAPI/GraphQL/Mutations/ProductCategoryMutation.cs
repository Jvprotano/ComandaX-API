using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.ProductCategories.Commands.CreateProductCategory;
using ComandaX.Application.Handlers.ProductCategories.Commands.UpdateProductCategory;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class ProductCategoryMutation
{
    public async Task<ProductCategoryDto> CreateProductCategoryAsync(
        [Service] ISender mediator,
        string name, string? icon)
    {
        return await mediator.Send(new CreateProductCategoryCommand(name, icon));
    }

    public async Task<bool> UpdateProductCategoryAsync(
        [Service] ISender mediator,
        Guid id,
        string? name,
        Optional<string?> icon)
    {
        await mediator.Send(new UpdateProductCategoryCommand(id, name, icon));
        return true;
    }
}

using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.ProductCategories.Commands.CreateProductCategory;
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
}

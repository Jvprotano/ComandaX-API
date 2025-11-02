using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.Products.Commands.CreateProduct;
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
}


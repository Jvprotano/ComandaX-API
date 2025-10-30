using ComandaX.Application.DTOs;
using ComandaX.Application.Products.Queries.GetProductById;
using ComandaX.Application.Products.Queries.GetProducts;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class ProductQuery
{
    public async Task<IList<ProductDto>> GetProductsAsync([Service] ISender mediator)
        => await mediator.Send(new GetProductsQuery());

    public async Task<ProductDto> GetProductByIdAsync(Guid id, [Service] ISender mediator)
        => await mediator.Send(new GetProductByIdQuery(id));
}

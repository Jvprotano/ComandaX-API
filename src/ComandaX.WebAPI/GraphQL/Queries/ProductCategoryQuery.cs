using ComandaX.Application.DTOs;
using ComandaX.Application.Handlers.ProductCategories.Queries.GetProductCategories;
using ComandaX.Application.Handlers.ProductCategories.Queries.GetProductCategoryById;
using MediatR;

namespace ComandaX.WebAPI.GraphQL.Queries;

[ExtendObjectType("Query")]
public class ProductCategoryQuery
{
    public async Task<IList<ProductCategoryDto>> GetProductCategoriesAsync([Service] ISender mediator)
    => await mediator.Send(new GetProductCategoriesQuery());

    public async Task<ProductCategoryDto> GetProductCategoryByIdAsync(Guid id, [Service] ISender mediator)
        => await mediator.Send(new GetProductCategoryByIdQuery(id));

}

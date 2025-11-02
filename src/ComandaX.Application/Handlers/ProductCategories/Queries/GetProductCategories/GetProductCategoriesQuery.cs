using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Queries.GetProductCategories;

public record GetProductCategoriesQuery : IRequest<IList<ProductCategoryDto>>;


using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Queries.GetProductCategoryById;

public sealed record GetProductCategoryByIdQuery(Guid Id) : IRequest<ProductCategoryDto>;

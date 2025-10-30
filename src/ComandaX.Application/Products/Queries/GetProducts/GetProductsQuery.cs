using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Products.Queries.GetProducts;

public record GetProductsQuery() : IRequest<IList<ProductDto>>;

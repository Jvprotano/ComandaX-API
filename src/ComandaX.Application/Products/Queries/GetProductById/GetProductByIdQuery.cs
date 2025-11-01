using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
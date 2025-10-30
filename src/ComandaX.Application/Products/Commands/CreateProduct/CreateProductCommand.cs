using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, decimal Price) : IRequest<ProductDto>;
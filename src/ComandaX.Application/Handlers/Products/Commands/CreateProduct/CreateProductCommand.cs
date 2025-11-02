using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, decimal Price, bool NeedPreparation = false) : IRequest<ProductDto>;
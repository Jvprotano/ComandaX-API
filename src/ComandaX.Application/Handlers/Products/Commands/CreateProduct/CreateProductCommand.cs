using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    Guid? ProductCategoryId,
    bool NeedPreparation = false,
    bool IsPricePerKg = false) : IRequest<ProductDto>;
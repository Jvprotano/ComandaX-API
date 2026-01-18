
using MediatR;
using HotChocolate;

namespace ComandaX.Application.Handlers.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string? Name,
    decimal? Price,
    bool? NeedPreparation,
    bool? IsPricePerKg,
    Optional<Guid?> ProductCategoryId) : IRequest<Unit>;
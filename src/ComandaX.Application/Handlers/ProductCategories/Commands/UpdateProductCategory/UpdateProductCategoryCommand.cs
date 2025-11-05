
using HotChocolate;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.UpdateProductCategory;

public record UpdateProductCategoryCommand(
    Guid Id,
    string? Name,
    Optional<string?> Icon) : IRequest<Unit>;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.DeleteProductCategory;

public sealed record DeleteProductCategoryCommand(Guid Id) : IRequest;
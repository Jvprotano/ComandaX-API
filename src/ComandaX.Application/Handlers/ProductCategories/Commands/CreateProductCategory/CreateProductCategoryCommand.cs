
using ComandaX.Application.DTOs;
using MediatR;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.CreateProductCategory;

public sealed record CreateProductCategoryCommand(string Name, string? Icon) : IRequest<ProductCategoryDto>;

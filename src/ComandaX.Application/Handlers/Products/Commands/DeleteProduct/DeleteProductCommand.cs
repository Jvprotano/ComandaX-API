using MediatR;

namespace ComandaX.Application.Handlers.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : IRequest;
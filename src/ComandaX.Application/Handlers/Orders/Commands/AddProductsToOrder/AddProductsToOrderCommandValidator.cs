using FluentValidation;

namespace ComandaX.Application.Handlers.Orders.Commands.AddProductsToOrder;

public class AddProductsToOrderCommandValidator : AbstractValidator<AddProductsToOrderCommand>
{
    public AddProductsToOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required");

        RuleFor(x => x.ProductIds)
            .NotNull().WithMessage("Product IDs list is required")
            .NotEmpty().WithMessage("At least one product ID is required");

        RuleForEach(x => x.ProductIds)
            .NotEmpty().WithMessage("Product ID cannot be empty");
    }
}


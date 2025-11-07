using FluentValidation;

namespace ComandaX.Application.Handlers.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Products)
            .NotNull().WithMessage("Products list is required")
            .NotEmpty().WithMessage("Order must have at least one product");

        RuleForEach(x => x.Products).ChildRules(product =>
        {
            product.RuleFor(p => p.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            product.RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero")
                .LessThanOrEqualTo(1000).WithMessage("Quantity cannot exceed 1000");
        });
    }
}


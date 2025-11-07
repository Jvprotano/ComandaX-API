using FluentValidation;

namespace ComandaX.Application.Handlers.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Product name cannot be only whitespace");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero")
            .LessThanOrEqualTo(999999.99m).WithMessage("Price cannot exceed 999,999.99");
    }
}


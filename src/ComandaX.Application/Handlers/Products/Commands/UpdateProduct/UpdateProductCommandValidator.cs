using FluentValidation;

namespace ComandaX.Application.Handlers.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters")
            .Must(name => name == null || !string.IsNullOrWhiteSpace(name))
            .WithMessage("Product name cannot be only whitespace")
            .When(x => x.Name != null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero")
            .LessThanOrEqualTo(999999.99m).WithMessage("Price cannot exceed 999,999.99")
            .When(x => x.Price.HasValue);
    }
}


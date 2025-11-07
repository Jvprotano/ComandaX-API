using FluentValidation;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product category name is required")
            .MaximumLength(100).WithMessage("Product category name cannot exceed 100 characters")
            .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Product category name cannot be only whitespace");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("Icon cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Icon));
    }
}


using FluentValidation;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.UpdateProductCategory;

public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    public UpdateProductCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product category ID is required");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Product category name cannot exceed 100 characters")
            .Must(name => name == null || !string.IsNullOrWhiteSpace(name))
            .WithMessage("Product category name cannot be only whitespace")
            .When(x => x.Name != null);

        RuleFor(x => x.Icon)
            .Must(icon => !icon.HasValue || icon.Value == null || icon.Value.Length <= 50)
            .WithMessage("Icon cannot exceed 50 characters")
            .When(x => x.Icon.HasValue);
    }
}


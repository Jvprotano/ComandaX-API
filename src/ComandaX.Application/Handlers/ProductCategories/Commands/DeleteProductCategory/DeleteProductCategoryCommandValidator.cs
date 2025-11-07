using FluentValidation;

namespace ComandaX.Application.Handlers.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandValidator : AbstractValidator<DeleteProductCategoryCommand>
{
    public DeleteProductCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product category ID is required");
    }
}


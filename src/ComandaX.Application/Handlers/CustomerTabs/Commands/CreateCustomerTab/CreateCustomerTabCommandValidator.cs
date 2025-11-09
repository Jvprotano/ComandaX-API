using FluentValidation;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;

public class CreateCustomerTabCommandValidator : AbstractValidator<CreateCustomerTabCommand>
{
    public CreateCustomerTabCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Customer name is required")
            .MaximumLength(50).WithMessage("Customer name cannot exceed 50 characters");
    }
}

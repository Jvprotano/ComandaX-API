using FluentValidation;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CreateCustomerTab;

public class CreateCustomerTabCommandValidator : AbstractValidator<CreateCustomerTabCommand>
{
    public CreateCustomerTabCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("Customer name cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));
    }
}


using FluentValidation;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.CloseCustomerTab;

public class CloseCustomerTabCommandValidator : AbstractValidator<CloseCustomerTabCommand>
{
    public CloseCustomerTabCommandValidator()
    {
        RuleFor(x => x.CustomerTabId)
            .NotEmpty()
            .WithMessage("Customer tab ID is required");
    }
}


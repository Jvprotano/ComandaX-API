using FluentValidation;

namespace ComandaX.Application.Handlers.CustomerTabs.Commands.DeleteCustomerTab;

public class DeleteCustomerTabCommandValidator : AbstractValidator<DeleteCustomerTabCommand>
{
    public DeleteCustomerTabCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Customer Tab ID is required");
    }
}


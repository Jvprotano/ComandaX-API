using FluentValidation;

namespace ComandaX.Application.Handlers.Orders.Commands.CloseOrder;

public class CloseOrderCommandValidator : AbstractValidator<CloseOrderCommand>
{
    public CloseOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required");
    }
}

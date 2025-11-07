using FluentValidation;

namespace ComandaX.Application.Handlers.Tables.Commands.UpdateTable;

public class UpdateTableCommandValidator : AbstractValidator<UpdateTableCommand>
{
    public UpdateTableCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Table ID is required");

        RuleFor(x => x.Number)
            .GreaterThan(0).WithMessage("Table number must be greater than zero")
            .LessThanOrEqualTo(9999).WithMessage("Table number cannot exceed 9999");
    }
}


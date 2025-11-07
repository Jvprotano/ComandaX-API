using FluentValidation;

namespace ComandaX.Application.Handlers.Tables.Commands.CreateTable;

public class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator()
    {
        RuleFor(x => x.Number)
            .GreaterThan(0).WithMessage("Table number must be greater than zero")
            .LessThanOrEqualTo(9999).WithMessage("Table number cannot exceed 9999")
            .When(x => x.Number.HasValue);
    }
}


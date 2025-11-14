using ComandaX.Application.Handlers.Orders.Commands.DeleteOrder;
using FluentValidation.TestHelper;
using Xunit;

namespace ComandaX.Tests.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandValidatorTests
{
    private readonly DeleteOrderCommandValidator _validator;

    public DeleteOrderCommandValidatorTests()
    {
        _validator = new DeleteOrderCommandValidator();
    }

    [Fact]
    public void Validate_ValidOrderId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new DeleteOrderCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyOrderId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteOrderCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}


using ComandaX.Application.Handlers.Orders.Commands.CreateOrder;
using FluentValidation.TestHelper;
using Xunit;

namespace ComandaX.Tests.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidatorTests
{
    private readonly CreateOrderCommandValidator _validator;

    public CreateOrderCommandValidatorTests()
    {
        _validator = new CreateOrderCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new List<CreateOrderProductDto>
            {
                new(Guid.NewGuid(), 1)
            }
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyProducts_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new List<CreateOrderProductDto>()
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Products);
    }

    [Fact]
    public void Validate_NullProducts_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            null!
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Products);
    }

    [Fact]
    public void Validate_ProductWithZeroQuantity_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new List<CreateOrderProductDto>
            {
                new(Guid.NewGuid(), 0)
            }
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Products[0].Quantity");
    }

    [Fact]
    public void Validate_ProductWithEmptyProductId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new List<CreateOrderProductDto>
            {
                new(Guid.Empty, 1)
            }
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Products[0].ProductId");
    }
}


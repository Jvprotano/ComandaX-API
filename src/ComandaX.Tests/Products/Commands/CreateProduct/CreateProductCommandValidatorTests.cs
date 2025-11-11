using ComandaX.Application.Handlers.Products.Commands.CreateProduct;
using FluentValidation.TestHelper;
using Xunit;

namespace ComandaX.Tests.Products.Commands.CreateProduct;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", 10.99m, null, false);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommand("", 10.99m, null, false);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_NullName_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommand(null!, 10.99m, null, false);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_ZeroPrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", 0m, null, false);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_NegativePrice_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", -5m, null, false);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }
}


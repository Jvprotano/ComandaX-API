using ComandaX.Application.Handlers.CustomerTabs.Commands.CloseCustomerTab;
using FluentValidation.TestHelper;
using Xunit;

namespace ComandaX.Tests.CustomerTabs.Commands.CloseCustomerTab;

public class CloseCustomerTabCommandValidatorTests
{
    private readonly CloseCustomerTabCommandValidator _validator;

    public CloseCustomerTabCommandValidatorTests()
    {
        _validator = new CloseCustomerTabCommandValidator();
    }

    [Fact]
    public void Validate_ValidCustomerTabId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CloseCustomerTabCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyCustomerTabId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CloseCustomerTabCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerTabId)
            .WithErrorMessage("Customer tab ID is required");
    }
}


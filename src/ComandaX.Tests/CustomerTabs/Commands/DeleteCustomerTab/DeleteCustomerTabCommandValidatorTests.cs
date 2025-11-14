using ComandaX.Application.Handlers.CustomerTabs.Commands.DeleteCustomerTab;
using FluentValidation.TestHelper;
using Xunit;

namespace ComandaX.Tests.CustomerTabs.Commands.DeleteCustomerTab;

public class DeleteCustomerTabCommandValidatorTests
{
    private readonly DeleteCustomerTabCommandValidator _validator;

    public DeleteCustomerTabCommandValidatorTests()
    {
        _validator = new DeleteCustomerTabCommandValidator();
    }

    [Fact]
    public void Validate_ValidCustomerTabId_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new DeleteCustomerTabCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyCustomerTabId_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteCustomerTabCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}


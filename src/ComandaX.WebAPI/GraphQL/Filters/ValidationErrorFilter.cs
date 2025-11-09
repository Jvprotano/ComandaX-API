using ComandaX.Application.Exceptions;
using FluentValidation;

namespace ComandaX.WebAPI.GraphQL.Filters;

/// <summary>
/// GraphQL error filter that handles application exceptions
/// and converts them to GraphQL errors with proper formatting.
/// </summary>
public class ValidationErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        // Handle FluentValidation exceptions
        if (error.Exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                })
                .ToList();

            return ErrorBuilder.New()
                .SetMessage("Validation failed")
                .SetCode("VALIDATION_ERROR")
                .SetExtension("validationErrors", errors)
                .Build();
        }

        // Handle RecordNotFoundException
        if (error.Exception is RecordNotFoundException notFoundException)
        {
            return ErrorBuilder.New()
                .SetMessage(notFoundException.Message)
                .SetCode("NOT_FOUND")
                .Build();
        }

        // Handle UserNotAuthorizedException
        if (error.Exception is UserNotAuthorizedException unauthorizedException)
        {
            return ErrorBuilder.New()
                .SetMessage(unauthorizedException.Message)
                .SetCode("UNAUTHORIZED")
                .Build();
        }

        // Handle OrderWithoutProductsException
        if (error.Exception is OrderWithoutProductsException orderException)
        {
            return ErrorBuilder.New()
                .SetMessage(orderException.Message)
                .SetCode("BUSINESS_RULE_VIOLATION")
                .Build();
        }

        return error;
    }
}


using System.Net;
using System.Text.Json;
using ComandaX.Application.Exceptions;

namespace ComandaX.WebAPI.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly IHostEnvironment _env = env;
    private JsonSerializerOptions _options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserNotAuthorizedException ex)
        {
            _logger.LogWarning(ex, "User not authorized: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var response = new ApiException(context.Response.StatusCode, ex.Message);

            var json = JsonSerializer.Serialize(response, _options);
            await context.Response.WriteAsync(json);
        }
        catch (RecordNotFoundException ex)
        {
            _logger.LogWarning(ex, "Record not found: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var response = new ApiException(context.Response.StatusCode, ex.Message);

            var json = JsonSerializer.Serialize(response, _options);
            await context.Response.WriteAsync(json);
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = ex.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage)).ToList();
            var response = new ValidationApiException(context.Response.StatusCode, "Validation failed", errors);

            var json = JsonSerializer.Serialize(response, _options);
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, "An unexpected error occurred.");

            var json = JsonSerializer.Serialize(response, _options);
            await context.Response.WriteAsync(json);
        }
    }
}

public class ApiException(int statusCode, string message, string? details = null)
{
    public int StatusCode { get; } = statusCode;
    public string Message { get; } = message;
    public string? Details { get; } = details;
}

public class ValidationApiException(int statusCode, string message, List<ValidationError> errors) : ApiException(statusCode, message)
{
    public List<ValidationError> Errors { get; } = errors;
}

public record ValidationError(string PropertyName, string ErrorMessage);

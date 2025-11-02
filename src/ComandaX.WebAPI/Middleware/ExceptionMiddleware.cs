using System.Net;
using System.Text.Json;
using ComandaX.Application.Exceptions;

namespace ComandaX.WebAPI.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    private JsonSerializerOptions _options = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

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

public class ApiException
{
    public int StatusCode { get; }
    public string Message { get; }
    public string? Details { get; }

    public ApiException(int statusCode, string message, string? details = null)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }
}

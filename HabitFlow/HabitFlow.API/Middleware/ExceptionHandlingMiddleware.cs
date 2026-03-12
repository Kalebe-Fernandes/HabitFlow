using HabitFlow.Domain.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace HabitFlow.API.Middleware
{
    /// <summary>
    /// Global exception handler middleware.
    /// Maps known exception types to appropriate HTTP responses.
    /// Prevents stack traces from being exposed to callers.
    /// </summary>
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Domain validation error on field: {Field}", ex.Source);
                await WriteResponseAsync(context, HttpStatusCode.BadRequest, "VALIDATION_ERROR", ex.Message);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Domain rule violation: {Code}", ex.ErrorCode);
                await WriteResponseAsync(context, HttpStatusCode.UnprocessableEntity, ex.ErrorCode, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                await WriteResponseAsync(context, HttpStatusCode.Forbidden, "FORBIDDEN", "Access denied");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteResponseAsync(context, HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "An unexpected error occurred");
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, string code, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var body = JsonSerializer.Serialize(new { code, message }, SerializerOptions);
            await context.Response.WriteAsync(body);
        }
    }
}

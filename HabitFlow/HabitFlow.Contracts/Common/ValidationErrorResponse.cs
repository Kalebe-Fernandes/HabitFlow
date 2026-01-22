namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Validation error response with field-specific errors.
    /// </summary>
    public record ValidationErrorResponse(
        string Message,
        Dictionary<string, string[]> Errors,
        string? TraceId = null
    ) : ErrorResponse("VALIDATION_ERROR", Message, Errors, TraceId);
}

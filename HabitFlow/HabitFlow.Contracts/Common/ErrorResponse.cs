namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Generic error response DTO.
    /// </summary>
    public record ErrorResponse(
        string Code,
        string Message,
        Dictionary<string, string[]>? Errors = null,
        string? TraceId = null
    );
}

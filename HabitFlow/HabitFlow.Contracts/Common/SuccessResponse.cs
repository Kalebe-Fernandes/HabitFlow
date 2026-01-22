namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Generic success response for operations that don't return data.
    /// </summary>
    public record SuccessResponse(
        string Message,
        bool Success = true
    );
}

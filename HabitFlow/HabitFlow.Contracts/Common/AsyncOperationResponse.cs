namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Response for async operations that are queued.
    /// </summary>
    public record AsyncOperationResponse(
        string OperationId,
        string Status, // Queued, Processing, Completed, Failed
        string? ResultUrl = null,
        DateTime? EstimatedCompletionTime = null
    );
}

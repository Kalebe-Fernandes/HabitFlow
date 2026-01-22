namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Batch operation result DTO.
    /// </summary>
    public record BatchOperationResult<T>(
        int TotalItems,
        int SuccessfulItems,
        int FailedItems,
        T[] SuccessfulResults,
        BatchOperationError[] Errors
    );
}

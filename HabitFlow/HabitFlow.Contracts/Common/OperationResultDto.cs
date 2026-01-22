namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Response for operations with a result value.
    /// </summary>
    public record OperationResultDto<T>(
        bool Success,
        T? Data,
        string? Message = null,
        string? ErrorCode = null
    );
}

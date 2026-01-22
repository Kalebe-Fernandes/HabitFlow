namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Error for individual items in a batch operation.
    /// </summary>
    public record BatchOperationError(
        int Index,
        string ItemIdentifier,
        string ErrorCode,
        string ErrorMessage
    );
}

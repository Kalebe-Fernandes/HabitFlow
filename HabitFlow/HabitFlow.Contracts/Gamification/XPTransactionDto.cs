namespace HabitFlow.Contracts.Gamification
{
    public record XPTransactionDto(
        long Id,
        int Amount,
        string Reason,
        Guid? RelatedEntityId,
        DateTime CreatedAt
    );
}

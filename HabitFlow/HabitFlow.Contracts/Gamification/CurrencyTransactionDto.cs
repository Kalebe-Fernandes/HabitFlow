namespace HabitFlow.Contracts.Gamification
{
    public record CurrencyTransactionDto(
        long Id,
        string Type, // Earned, Spent
        int Amount,
        int BalanceAfter,
        string Reason,
        DateTime CreatedAt
    );
}

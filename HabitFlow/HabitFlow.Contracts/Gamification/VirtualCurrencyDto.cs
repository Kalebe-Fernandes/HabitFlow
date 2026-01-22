namespace HabitFlow.Contracts.Gamification
{
    // Currency DTOs
    public record VirtualCurrencyDto(
        Guid UserId,
        int CurrentBalance,
        int TotalEarned,
        int TotalSpent
    );
}

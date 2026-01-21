namespace HabitFlow.Contracts.Gamification
{
    public record UserBadgeDto(
        long Id,
        BadgeDto Badge,
        DateTime EarnedAt
    );
}

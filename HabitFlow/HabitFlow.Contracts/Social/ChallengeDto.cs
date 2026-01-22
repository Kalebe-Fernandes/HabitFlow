namespace HabitFlow.Contracts.Social
{
    /// <summary>
    /// Response DTO for group challenges (RF-023).
    /// </summary>
    public record ChallengeDto(
        Guid Id,
        string Name,
        string Description,
        Guid? GroupId,
        string? GroupName,
        int TargetValue,
        string Unit,
        DateTime StartDate,
        DateTime EndDate,
        int ParticipantCount,
        int CurrentProgress,
        decimal ProgressPercentage,
        string Status, // NotStarted, Active, Completed, Failed
        ChallengeParticipantDto[] TopParticipants
    );
}

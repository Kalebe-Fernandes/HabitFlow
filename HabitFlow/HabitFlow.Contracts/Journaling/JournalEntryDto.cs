namespace HabitFlow.Contracts.Journaling
{
    /// <summary>
    /// Response DTO for journal entries (RF-019).
    /// </summary>
    public record JournalEntryDto(
        long Id,
        Guid UserId,
        DateTime Date,
        string Content,
        int? MoodLevel, // 1-5 (RF-020)
        int? EnergyLevel, // 1-5 (RF-020)
        string[] AttachedImageUrls, // RF-035
        string[] AttachedAudioUrls, // RF-035
        bool IsPrivate,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}

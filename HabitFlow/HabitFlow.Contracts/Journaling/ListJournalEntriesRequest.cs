namespace HabitFlow.Contracts.Journaling
{
    /// <summary>
    /// Request DTO for listing journal entries.
    /// </summary>
    public record ListJournalEntriesRequest(
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        int? MinMoodLevel = null,
        int? MaxMoodLevel = null,
        int Page = 1,
        int PageSize = 20
    );
}

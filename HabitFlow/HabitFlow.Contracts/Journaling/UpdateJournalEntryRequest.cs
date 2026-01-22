namespace HabitFlow.Contracts.Journaling
{
    /// <summary>
    /// Request DTO for updating a journal entry.
    /// </summary>
    public record UpdateJournalEntryRequest(
        string Content,
        int? MoodLevel = null,
        int? EnergyLevel = null,
        bool? IsPrivate = null
    );
}

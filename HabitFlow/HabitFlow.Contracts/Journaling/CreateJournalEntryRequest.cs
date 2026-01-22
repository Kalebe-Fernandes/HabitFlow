namespace HabitFlow.Contracts.Journaling
{
    /// <summary>
    /// Request DTO for creating a journal entry.
    /// </summary>
    public record CreateJournalEntryRequest(
        DateTime Date,
        string Content,
        int? MoodLevel = null, // 1-5
        int? EnergyLevel = null, // 1-5
        bool IsPrivate = true
    );
}

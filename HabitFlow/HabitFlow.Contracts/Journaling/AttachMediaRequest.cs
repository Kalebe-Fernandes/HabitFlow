namespace HabitFlow.Contracts.Journaling
{
    /// <summary>
    /// Request DTO for attaching media to journal entry (RF-035).
    /// </summary>
    public record AttachMediaRequest(
        long JournalEntryId,
        string MediaType, // Image, Audio
        string MediaUrl
    );
}

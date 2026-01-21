namespace HabitFlow.Contracts.Analytics
{
    public record ExportDataRequest(
        string Format, // "CSV", "JSON", "PDF"
        DateTime StartDate,
        DateTime EndDate,
        bool IncludeHabits = true,
        bool IncludeGoals = true,
        bool IncludeGamification = true,
        bool IncludeJournal = false
    );
}

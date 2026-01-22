namespace HabitFlow.Contracts.Templates
{
    /// <summary>
    /// Request DTO for adding a template as a habit.
    /// </summary>
    public record AddTemplateAsHabitRequest(
        int TemplateId,
        DateTime? CustomStartDate = null,
        DateTime? CustomEndDate = null,
        int? CustomXpReward = null
    );
}

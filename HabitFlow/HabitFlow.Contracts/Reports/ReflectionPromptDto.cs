namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Response DTO for reflection prompts (RF-045).
    /// </summary>
    public record ReflectionPromptDto(
        string PromptId,
        string Question,
        string Category, // Motivation, Progress, Alignment, Feelings
        DateTime GeneratedAt,
        string[]? SuggestedAnswers // Optional multiple choice
    );
}

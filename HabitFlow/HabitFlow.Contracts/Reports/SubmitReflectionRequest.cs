namespace HabitFlow.Contracts.Reports
{
    /// <summary>
    /// Request DTO for submitting reflection response.
    /// </summary>
    public record SubmitReflectionRequest(
        string PromptId,
        string Response,
        int? SentimentScore = null // 1-5
    );
}

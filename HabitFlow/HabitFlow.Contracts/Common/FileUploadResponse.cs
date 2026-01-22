namespace HabitFlow.Contracts.Common
{
    /// <summary>
    /// Response for file upload operations.
    /// </summary>
    public record FileUploadResponse(
        string FileId,
        string FileName,
        string FileUrl,
        long FileSizeBytes,
        string ContentType,
        DateTime UploadedAt
    );
}

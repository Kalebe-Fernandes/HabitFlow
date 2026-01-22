namespace HabitFlow.Contracts.Analytics
{
    /// <summary>
    /// Request DTO for data export (LGPD compliance - RF-033).
    /// </summary>
    public record ExportDataResponse(
        string ExportId,
        string Format, // CSV, JSON, PDF
        string DownloadUrl,
        DateTime GeneratedAt,
        DateTime ExpiresAt,
        long FileSizeBytes
    );
}

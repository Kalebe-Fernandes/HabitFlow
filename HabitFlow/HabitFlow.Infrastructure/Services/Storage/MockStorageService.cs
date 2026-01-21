using Microsoft.Extensions.Logging;

namespace HabitFlow.Infrastructure.Services.Storage
{
    public class MockStorageService(ILogger<MockStorageService> logger) : IStorageService
    {
        private readonly ILogger<MockStorageService> _logger = logger;

        public Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var mockUrl = $"https://mockcdn.habitflow.com/{Guid.NewGuid()}/{fileName}";
            _logger.LogInformation("[MOCK STORAGE] File uploaded: {FileName} to {Url}", fileName, mockUrl);
            return Task.FromResult(mockUrl);
        }

        public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("[MOCK STORAGE] File deleted: {Url}", fileUrl);
            return Task.CompletedTask;
        }
    }
}

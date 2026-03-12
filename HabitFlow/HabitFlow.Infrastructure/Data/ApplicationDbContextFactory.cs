using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HabitFlow.Infrastructure.Data
{
    public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environment = GetEnvironment(args);

            var basePath = ResolveBasePath();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "ConnectionStrings:DefaultConnection não foi encontrada para o design-time do EF Core.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30);
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        private static string GetEnvironment(string[] args)
        {
            var environmentArg = args.FirstOrDefault(a => a.StartsWith("--environment=", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(environmentArg))
            {
                return environmentArg.Split('=', 2)[1];
            }

            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                   ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                   ?? "Development";
        }

        private static string ResolveBasePath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var apiProjectPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", "HabitFlow.API"));
            if (Directory.Exists(apiProjectPath))
            {
                return apiProjectPath;
            }

            return currentDirectory;
        }
    }
}

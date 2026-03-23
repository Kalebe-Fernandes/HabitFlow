using HabitFlow.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HabitFlow.IntegrationTests.Infrastructure
{
    /// <summary>
    /// WebApplicationFactory customized for integration tests.
    /// Replaces SQL Server with an isolated in-memory SQLite database
    /// so that tests run without any external dependencies.
    /// </summary>
    public class HabitFlowWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove the SQL Server DbContext registration
                services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                services.RemoveAll<ApplicationDbContext>();

                // Replace with SQLite in-memory per test run
                var dbName = $"HabitFlowTest_{Guid.NewGuid()}";
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(dbName));

                // Remove all background hosted services to avoid timer interference in tests
                services.RemoveAll<Microsoft.Extensions.Hosting.IHostedService>();

                // Ensure the in-memory database is created with the correct schema
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            });
        }

        /// <summary>
        /// Creates a new scoped service provider for each test requiring direct
        /// access to the database or services.
        /// </summary>
        public IServiceScope CreateServiceScope() => Services.CreateScope();
    }
}

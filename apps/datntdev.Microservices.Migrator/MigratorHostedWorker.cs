using datntdev.Microservices.Identity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace datntdev.Microservices.Migrator
{
    internal class MigratorHostedWorker(IServiceProvider services) : IHostedService, IDisposable
    {
        private Timer? _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a timer that will run after 1 second to allow the application to start up
            // and then stop the application lifetime to allow migrations to complete.
            // We use a timer to ensure that the application has fully started before we attempt to run migrations.
            _timer = new Timer(async (services) =>
            {
                var scoped = ((IServiceProvider)services!).CreateScope().ServiceProvider;
                var logger = scoped.GetRequiredService<ILogger<MigratorHostedWorker>>();
                var lifetime = scoped.GetRequiredService<IHostApplicationLifetime>();

                logger.LogInformation("Migrator service is starting...");

                logger.LogInformation("Checking database existed or pending migrations...");
                var dbContext = scoped.GetRequiredService<IdentityApplicationDbContext>();
                var pendingChanges = dbContext.Database.GetPendingMigrations();
                if (pendingChanges.Any()) dbContext.Database.Migrate();

                logger.LogInformation("Seeding initial data...");
                await Parallel.ForEachAsync(GetDataSeeders(scoped), cancellationToken, async (seeder, ct) =>
                {
                    var seederName = seeder.GetType().Name;
                    await seeder.SeedDataAsync(ct);
                    logger.LogInformation("Data seeding completed for {seederName}.", seederName);
                });

                logger.LogInformation("Stopping application lifetime to allow migration to complete.");
                lifetime.StopApplication();
            }, services, TimeSpan.FromSeconds(1), Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();

        private static IEnumerable<Seeders.IDataSeeder> GetDataSeeders(IServiceProvider services)
        {
            return 
            [
                new Seeders.IdentityDataSeeder(services),
            ];
        }
    }
}

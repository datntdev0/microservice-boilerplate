using System.Threading;

namespace datntdev.Microservices.Migrator.Seeders
{
    internal interface IDataSeeder
    {
        Task SeedDataAsync(CancellationToken cancellationToken);
    }
}

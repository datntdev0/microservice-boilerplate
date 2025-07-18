using datntdev.Microservices.Common.Registars;
using datntdev.Microservices.Identity.Application.MultiTenancy.Models;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Identity.Application.MultiTenancy
{
    [InjectService(ServiceLifetime.Scoped)]
    public class AppTenantManager(IServiceProvider services)
    {
        private readonly IdentityApplicationDbContext _dbContext = services.GetRequiredService<IdentityApplicationDbContext>();

        public Task<AppTenantEntity?> GetAppTenantAsync(int id)
        {
            return _dbContext.AppTenants.FindAsync(id).AsTask();
        }

        public async Task<AppTenantEntity> CreateAsync(AppTenantEntity input, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var newEntity = await _dbContext.AppTenants.AddAsync(input, cancellationToken).AsTask();
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return newEntity.Entity;
        }
    }
}

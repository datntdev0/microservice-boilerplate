using datntdev.Microservices.Common.Helpers;
using datntdev.Microservices.Common.Registars;
using datntdev.Microservices.Identity.Application.MultiTenancy.Models;
using datntdev.Microservices.Identity.Contracts.MultiTenancy.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Identity.Application.MultiTenancy
{
    [InjectService(ServiceLifetime.Scoped)]
    public class TenantManager(IServiceProvider services)
    {
        private readonly IdentityApplicationDbContext _dbContext = services.GetRequiredService<IdentityApplicationDbContext>();

        public Task<AppTenantEntity?> GetAppTenantAsync(int id)
        {
            return _dbContext.AppTenants.FindAsync(id).AsTask();
        }

        public async Task<(int, AppTenantEntity[])> GetAppTenantsAsync(TenantListRequest input)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            var query = _dbContext.AppTenants.AsQueryable()
                .Search(input, x => x.Name.Contains(input.Search!));

            var count = await query.CountAsync();
            var items = await query.OrderBy(input).Paging(input).ToArrayAsync();

            await transaction.CommitAsync();

            return (count, items);
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

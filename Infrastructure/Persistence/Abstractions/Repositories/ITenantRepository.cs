using Infrastructure.Domain.Tenants;

namespace Infrastructure.Persistence.Abstractions;

public interface ITenantRepository
{
    Task<Tenant[]> GetPagedList();

    Task<Tenant?> GetByIdAsync(Guid id);
    
    Task<Tenant?> GetByUserIdAsync(Guid id);

    Task<Tenant> CreateAsync(Tenant tenant);

    Task RemoveAsync(Guid id);
}
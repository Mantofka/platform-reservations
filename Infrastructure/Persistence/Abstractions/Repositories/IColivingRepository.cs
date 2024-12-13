using Application.Contracts;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence.Abstractions.Models;
using Infrastructure.Persistence.Abstractions.Models.Coliving;

namespace Infrastructure.Persistence.Abstractions;

public interface IColivingRepository
{
    Task<Coliving[]> GetPagedList();
    
    Task<Coliving[]> GetPagedOwnerColivingList(Guid userId);

    Task<Coliving?> GetByIdAsync(Guid id);
    
    Task<Tenant[]> GetTenants(Guid id, Guid roomId);

    Task<Coliving> CreateAsync(Coliving coliving);

    Task RemoveAsync(Guid id);
    Task<Guid?> GetOwnerIdByColivingId(Guid id);
}
using Application.Contracts;
using Infrastructure.Domain.Maintenance;
using Infrastructure.Persistence.Abstractions.Models;
using Infrastructure.Persistence.Abstractions.Models.Coliving;

namespace Infrastructure.Persistence.Abstractions;

public interface IMaintenanceRepository
{
    Task<Maintenance[]> GetPagedList();

    Task<Maintenance?> GetByIdAsync(Guid id);
    
    Task<Maintenance[]> GetMaintenancesWithRoomAndTenantAsync(Guid colivingId, Guid roomId, Guid tenantId);

    Task<Maintenance> CreateAsync(Maintenance maintenance);

    Task RemoveAsync(Guid id);
}
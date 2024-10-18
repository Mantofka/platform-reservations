using Application.Contracts.Maintenance;

namespace Application.Abstractions.Tenant;

public interface IMaintenanceService
{
    Task<MaintenanceResponseDto[]> GetPagedList();

    Task<MaintenanceResponseDto?> FindById(Guid id);

    Task<MaintenanceResponseDto[]> GetMaintenances(Guid colivingId, Guid roomId, Guid tenantId);
    
    Task<MaintenanceResponseDto> Create(MaintenanceCreateDto input);
    
    Task<MaintenanceResponseDto?> Edit(Guid id, MaintenanceCreateDto input);
    
    Task<bool> Remove(Guid id);
}
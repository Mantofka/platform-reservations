using Application.Contracts.Tenant;

namespace Application.Abstractions.Tenant;

public interface ITenantService
{
    Task<TenantResponseDto[]> GetPagedList();

    Task<TenantResponseDto?> FindById(Guid id);
    
    Task<TenantResponseDto> Create(TenantCreateDto input);
    
    Task<TenantResponseDto?> Edit(Guid id, TenantUpdateDto input);
    
    Task<bool> Remove(Guid id);
    
    Task<TenantResponseDto> GetCurrentTenant(Guid userId);
}
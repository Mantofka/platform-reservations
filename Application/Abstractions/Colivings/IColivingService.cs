using Application.Contracts;
using Application.Contracts.Tenant;

namespace Application.Abstractions.Colivings;

public interface IColivingService
{
    Task<ColivingResponseDto[]> GetPagedList();
    
    Task<ColivingResponseDto[]> GetPagedOwnerColivings(Guid colivingId);

    Task<ColivingResponseDto?> FindById(Guid id);
    
    Task<TenantResponseDto[]> GetTenants(Guid id, Guid roomId);
    
    Task<ColivingResponseDto> Create(ColivingCreateDto input, Guid userId);
    
    Task<ColivingResponseDto?> Edit(Guid id, ColivingCreateDto input);
    
    Task<bool> Remove(Guid id);

    Task<Guid?> GetOwnerIdByColivingId(Guid colivingId);
}

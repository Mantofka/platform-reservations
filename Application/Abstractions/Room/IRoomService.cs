using Application.Contracts.Room;
using Application.Contracts.Tenant;

namespace Application.Abstractions.Room;

public interface IRoomService
{
    Task<RoomResponseDto[]> GetPagedList();

    Task<RoomResponseDto?> FindById(Guid id);
    
    Task<RoomResponseDto> Create(RoomCreateDto input);
    
    Task<RoomResponseDto?> Edit(Guid id, RoomCreateDto input);

    Task<RoomResponseDto> AssignTenant(AssignTenantDto input);
    
    Task<bool> Remove(Guid id);
}
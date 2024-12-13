using Application.Contracts.Room;
using Application.Contracts.Tenant;

namespace Application.Abstractions.Room;

public interface IRoomService
{
    Task<RoomResponseDto[]> GetPagedList();
    
    Task<RoomResponseDto[]> GetPagedColivingRoomsList(Guid colivingId);

    Task<RoomResponseDto?> FindById(Guid id);
    
    Task<RoomResponseDto> Create(RoomCreateDto input);
    
    Task<RoomResponseDto?> Edit(Guid id, RoomCreateDto input);

    Task AssignTenant(AssignTenantDto input, Guid userId);
    
    Task<bool> Remove(Guid id);
    
    Task<Guid?> GetOwnerIdByRoomId(Guid colivingId);
}
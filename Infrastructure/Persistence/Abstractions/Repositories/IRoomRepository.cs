using Application.Contracts.Room;
using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.RoomTenant;

namespace Infrastructure.Persistence.Abstractions;

public interface IRoomRepository
{
    Task<Room[]> GetPagedList();

    Task<bool> IsTenantAssignedToRoomAsync(Guid tenantId, Guid roomId);
    
    Task<Room[]> GetPagedColivingRoomList(Guid colivingId);

    Task<Room?> GetByIdAsync(Guid id);

    Task<Room> CreateAsync(Room coliving);
    
    Task AssignTenantAsync(RoomTenant tenant);

    Task RemoveAsync(Guid id);
    
    Task<Guid?> GetOwnerIdByRoomId(Guid id);
}
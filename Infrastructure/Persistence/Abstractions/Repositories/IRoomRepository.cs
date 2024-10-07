using Application.Contracts.Room;
using Infrastructure.Domain.Rooms;

namespace Infrastructure.Persistence.Abstractions;

public interface IRoomRepository
{
    Task<Room[]> GetPagedList();

    Task<Room?> GetByIdAsync(Guid id);

    Task<Room> CreateAsync(Room coliving);
    
    Task<Room> AssignTenantAsync(AssignTenantDto input);

    Task RemoveAsync(Guid id);
}
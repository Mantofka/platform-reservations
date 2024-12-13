using Application.Contracts.Room;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Infrastructure.Persistence.Abstractions.Models.Coliving;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Rooms.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ColivingReservationsDbContext _context;
    public RoomRepository(ColivingReservationsDbContext context)
    {
        _context = context;
    }
    public async Task<Room[]> GetPagedList()
    {
        var query = _context.Set<Room>().Include(x => x.Coliving).Include(x => x.Tenants).AsQueryable();
        return await query.ToArrayAsync().ConfigureAwait(false);
    }
    
    public async Task<bool> IsTenantAssignedToRoomAsync(Guid tenantId, Guid roomId)
    {
        return await _context.Set<RoomTenant.RoomTenant>()
            .AnyAsync(rt => rt.TenantId == tenantId && rt.RoomId == roomId)
            .ConfigureAwait(false);
    }
    
    public async Task<Room[]> GetPagedColivingRoomList(Guid colivingId)
    {
        var query = _context.Set<Room>().Include(x => x.Coliving).Where(r => r.ColivingId == colivingId).AsQueryable();
        return await query.ToArrayAsync().ConfigureAwait(false);
    }
    
    
    
    public async Task<Room> CreateAsync(Room tenant)
    {
        var coliving = await _context.Colivings
            .SingleOrDefaultAsync(c => c.Id == tenant.ColivingId)
            .ConfigureAwait(false);
        
        if (coliving == null)
        {
            throw new Exception("Coliving not found");
        }
        tenant.Coliving = coliving;
        await _context.AddAsync(tenant).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return tenant;
    }
    
    public async Task RemoveAsync(Guid id)
    {
        var entity = await _context.FindAsync<Room>([id]).ConfigureAwait(false);

        if (entity != null)
        {
            _context.Remove(entity);
        }
        else
        {
            throw new Exception("Room not found");
        }
    }

    public async Task AssignTenantAsync(RoomTenant.RoomTenant tenant)
    {
        await _context.AddAsync(tenant).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        
    }
    
    public async Task<Room?> GetByIdAsync(Guid id)
    {
        var result = await _context
            .Set<Room>()
            .Include(x => x.Coliving)
            .Include(x => x.Tenants)
            .SingleOrDefaultAsync(j => j.Id == id)
            .ConfigureAwait(false);

        return result;
    }
    
    public async Task<Guid?> GetOwnerIdByRoomId(Guid roomId)
    {
        var coliving = await _context.Set<Room>().Include(r => r.Coliving).Where(c => c.Id == roomId).FirstOrDefaultAsync();
        return coliving?.Coliving.UserId;
    }
}
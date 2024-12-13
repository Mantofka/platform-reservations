using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Tenants.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly ColivingReservationsDbContext _context;
    public TenantRepository(ColivingReservationsDbContext context)
    {
        _context = context;
    }
    public async Task<Tenant[]> GetPagedList()
    {
        var query = _context.Set<Tenant>().Include(x => x.Rooms).Include(x => x.User).AsQueryable();
        return await query.ToArrayAsync().ConfigureAwait(false);
    }
    
    public async Task<Tenant> CreateAsync(Tenant tenant)
    {
        if (tenant.Rooms != null && tenant.Rooms.Any())
        {
            foreach (var room in tenant.Rooms)
            {
                var existingRoom = await _context.Rooms.FindAsync(room.Id).ConfigureAwait(false);
                if (existingRoom != null)
                {
                    existingRoom.Tenants.Add(tenant);
                }
                else
                {
                    throw new Exception("Room not found");
                }
            }
        }
        await _context.Tenants.AddAsync(tenant).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return tenant;
    }
    
    public async Task RemoveAsync(Guid id)
    {
        var entity = await _context.FindAsync<Tenant>([id]).ConfigureAwait(false);

        if (entity != null)
        {
            _context.Remove(entity);
        }
        else
        {
            throw new Exception("Tenant not found");
        }
    }
    
    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        var result = await _context
            .Set<Tenant>()
            .Include(x => x.Rooms)
            .Include(x => x.User)
            .SingleOrDefaultAsync(j => j.Id == id)
            .ConfigureAwait(false);

        return result;
    }
    
    public async Task<Tenant?> GetByUserIdAsync(Guid id)
    {
        var result = await _context
            .Set<Tenant>()
            .Include(x => x.Rooms)
            .Include(x => x.User)
            .SingleOrDefaultAsync(j => j.UserId == id)
            .ConfigureAwait(false);

        return result;
    }
}
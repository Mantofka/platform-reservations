using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Maintenance.Repositories;

public class MaintenanceRepository : IMaintenanceRepository
{
    private readonly ColivingReservationsDbContext _context;
    public MaintenanceRepository(ColivingReservationsDbContext context)
    {
        _context = context;
    }
    public async Task<Maintenance[]> GetPagedList()
    {
        var query = _context.Set<Maintenance>().Include(x => x.Tenant).Include(x => x.AssignedRoom).AsQueryable();
        return await query.ToArrayAsync().ConfigureAwait(false);
    }
    
    public async Task<Maintenance> CreateAsync(Maintenance maintenance)
    {
        await _context.AddAsync(maintenance).ConfigureAwait(false);

        return maintenance;
    }
    
    public async Task RemoveAsync(Guid id)
    {
        var entity = await _context.FindAsync<Maintenance>([id]).ConfigureAwait(false);

        if (entity != null)
        {
            _context.Remove(entity);
        }
        else
        {
            throw new Exception("Maintenance not found");
        }
    }
    
    public async Task<Maintenance?> GetByIdAsync(Guid id)
    {
        var result = await _context
            .Set<Maintenance>()
            .Include(x => x.AssignedRoom)
            .Include(x => x.Tenant)
            .SingleOrDefaultAsync(j => j.Id == id)
            .ConfigureAwait(false);

        return result;
    }
    
    public async Task<Maintenance[]> GetMaintenancesWithRoomAndTenantAsync(Guid colivingId, Guid roomId, Guid tenantId)
    {
        return await _context.Maintenances
            .Include(m => m.AssignedRoom)
            .ThenInclude(m => m.Coliving)
            .Include(m => m.Tenant)
            .Where(m => m.AssignedRoom.Coliving.Id == colivingId && m.AssignedRoom.Id == roomId &&
                        m.Tenant.Id == tenantId).ToArrayAsync().ConfigureAwait(false);
    }
}
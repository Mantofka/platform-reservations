using Infrastructure.Domain.Rooms;
using Infrastructure.Domain.Tenants;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Infrastructure.Persistence.Abstractions.Models.Coliving;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.Colivings.Repositories;

public class ColivingRepository : IColivingRepository
{
    private readonly ColivingReservationsDbContext _context;

    public ColivingRepository(ColivingReservationsDbContext context)
    {
        _context = context;
    }

    public async Task<Coliving[]> GetPagedList()
    {
        var query = _context.Set<Coliving>().AsQueryable();
        return await query.ToArrayAsync().ConfigureAwait(false);
    }
    
    public async Task<Coliving[]> GetPagedOwnerColivingList(Guid userId)
    {
        var query = _context.Set<Coliving>().Where(c => c.UserId == userId).AsQueryable();
        return await query.ToArrayAsync().ConfigureAwait(false);
    }
    
    public async Task<Guid?> GetOwnerIdByColivingId(Guid colivingId)
    {
        var coliving = await _context.Set<Coliving>().Where(c => c.Id == colivingId).FirstOrDefaultAsync();
        return coliving?.UserId;
    }
    
    public async Task<Tenant[]> GetTenants(Guid id, Guid roomId)
    {
        var tenants = await _context.Set<RoomTenant.RoomTenant>()
            .Where(rt => rt.RoomId == roomId)
            .Include(rt => rt.Tenant)
            .ThenInclude(t => t.User)
            .Select(rt => rt.Tenant)
            .ToArrayAsync()
            .ConfigureAwait(false);
        
        if (!tenants.Any())
        {
            var colivingExists = await _context.Set<Coliving>()
                .AnyAsync(c => c.Id == id)
                .ConfigureAwait(false);

            if (!colivingExists)
            {
                throw new KeyNotFoundException($"Coliving with ID {id} not found.");
            }

            var roomExists = await _context.Set<Room>()
                .AnyAsync(r => r.Id == roomId && r.ColivingId == id)
                .ConfigureAwait(false);

            if (!roomExists)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found in Coliving with ID {id}.");
            }
        }
        return tenants;
    }
    
    public async Task<Coliving> CreateAsync(Coliving coliving)
    {
        await _context.AddAsync(coliving).ConfigureAwait(false);

        return coliving;
    }

    public async Task RemoveAsync(Guid id)
    {
        var entity = await _context.FindAsync<Coliving>([id]).ConfigureAwait(false);

        if (entity != null)
        {
            _context.Remove(entity);
        }
        else
        {
            throw new Exception("Coliving not found"); 
        }
    }
    
    public async Task<Coliving?> GetByIdAsync(Guid id)
    {
        var result = await _context
            .Set<Coliving>()
            .Include(x => x.Rooms)
            .SingleOrDefaultAsync(j => j.Id == id)
            .ConfigureAwait(false);

        return result;
    }
}
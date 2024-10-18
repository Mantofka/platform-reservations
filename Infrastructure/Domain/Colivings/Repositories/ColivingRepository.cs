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
    
    public async Task<Tenant[]> GetTenants(Guid id, Guid roomId)
    {
        var coliving = await _context.Set<Coliving>()
            .Include(c => c.Rooms)
            .ThenInclude(r => r.Tenants)
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);

        if (coliving == null)
        {
            throw new KeyNotFoundException($"Coliving with ID {id} not found.");
        }

        var room = coliving.Rooms.FirstOrDefault(r => r.Id == roomId);

        if (room == null)
        {
            throw new KeyNotFoundException($"Room with ID {roomId} not found in Coliving with ID {id}.");
        }

        return room.Tenants.ToArray();
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
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
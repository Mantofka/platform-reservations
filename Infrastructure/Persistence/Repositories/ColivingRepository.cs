using Infrastructure.Persistence.Abstractions;
using Infrastructure.Persistence.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

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
}
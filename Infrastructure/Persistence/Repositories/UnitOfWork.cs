using Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ColivingReservationsDbContext _context;
    private readonly IColivingRepository _colivingRepository;
    
    public UnitOfWork(ColivingReservationsDbContext context)
    {
        _context = context;
        _colivingRepository = new ColivingRepository(context);
    }
    
    public IColivingRepository GetColivings()
    {
        return _colivingRepository;
    }
    
    public async Task Commit()
    {
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public async Task Reject()
    {

        foreach (var item in _context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged))
        {
            switch (item.State)
            {
                case EntityState.Added:
                    item.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                case EntityState.Modified:
                    await item.ReloadAsync().ConfigureAwait(false);
                    break;
            }
        }
    }
}
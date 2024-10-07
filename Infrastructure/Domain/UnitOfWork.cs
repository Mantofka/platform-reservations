using Infrastructure.Domain.Colivings.Repositories;
using Infrastructure.Domain.Rooms.Repositories;
using Infrastructure.Domain.Tenants.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Domain;

public class UnitOfWork : IUnitOfWork
{
    private readonly ColivingReservationsDbContext _context;
    private readonly IColivingRepository _colivingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ITenantRepository _tenantRepository;
    
    public UnitOfWork(ColivingReservationsDbContext context)
    {
        _context = context;
        _colivingRepository = new ColivingRepository(context);
        _roomRepository = new RoomRepository(context);
        _tenantRepository = new TenantRepository(context);
    }
    
    public IColivingRepository GetColivings()
    {
        return _colivingRepository;
    }
    
    public IRoomRepository GetRooms()
    {
        return _roomRepository;
    }
    
    public ITenantRepository GetTenants()
    {
        return _tenantRepository;
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
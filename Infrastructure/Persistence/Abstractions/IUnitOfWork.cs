namespace Infrastructure.Persistence.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IColivingRepository GetColivings();

    IRoomRepository GetRooms();

    ITenantRepository GetTenants();
    
    
    Task Commit();

    Task Reject();
}
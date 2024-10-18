namespace Infrastructure.Persistence.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IColivingRepository GetColivings();

    IRoomRepository GetRooms();

    ITenantRepository GetTenants();
    
    IMaintenanceRepository GetMaintenances();
    
    
    Task Commit();

    Task Reject();
}
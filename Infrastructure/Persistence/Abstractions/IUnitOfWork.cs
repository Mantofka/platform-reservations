namespace Infrastructure.Persistence.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IColivingRepository GetColivings();
    
    Task Commit();

    Task Reject();
}
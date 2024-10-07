using Infrastructure.Persistence.Abstractions.Models;

namespace Infrastructure.Persistence.Abstractions;

public interface IColivingRepository
{
    Task<Coliving[]> GetPagedList();
}
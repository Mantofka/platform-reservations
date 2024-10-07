using Application.Contracts;
using Infrastructure.Persistence.Abstractions.Models;
using Infrastructure.Persistence.Abstractions.Models.Coliving;

namespace Infrastructure.Persistence.Abstractions;

public interface IColivingRepository
{
    Task<Coliving[]> GetPagedList();

    Task<Coliving?> GetByIdAsync(Guid id);

    Task<Coliving> CreateAsync(Coliving coliving);

    Task RemoveAsync(Guid id);
}
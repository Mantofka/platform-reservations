using Infrastructure.Domain.User;

namespace Infrastructure.Persistence.Abstractions;

public interface IUserRepository
{
    Task<User[]> GetColivingOwners();
}
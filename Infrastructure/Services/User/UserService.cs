using Application.Abstractions.User;
using Application.Contracts.Auth;
using Infrastructure.Persistence.Abstractions;

namespace Infrastructure.Services.User;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
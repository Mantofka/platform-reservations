using System.Security.Claims;

namespace Application.Abstractions.User;

public interface IUserContextService
{
    ClaimsPrincipal GetUser();
    
    Guid GetUserId();
    
    string GetUserRole();
}
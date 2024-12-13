using System.Security.Claims;
using Application.Abstractions.User;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.User;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public ClaimsPrincipal GetUser()
    {
        return _httpContextAccessor.HttpContext?.User;
    }
    
    public Guid GetUserId()
    {
        var userIdClaim = GetUser()?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdClaim != null ? Guid.Parse(userIdClaim) : Guid.Empty;
    }
    
    public string GetUserRole()
    {
        return GetUser()?.FindFirst(ClaimTypes.Role)?.Value!;
    }
}
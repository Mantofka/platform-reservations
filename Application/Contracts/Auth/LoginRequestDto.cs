using Shared.Roles;

namespace Application.Contracts.Auth;

public class LoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public RolesTypes Role { get; set; }
}
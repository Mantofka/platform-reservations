using Shared.Roles;

namespace Application.Contracts.Auth;

public class RegistrationCreateDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public RolesTypes Role { get; set; }
}
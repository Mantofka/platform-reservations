using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Domain.User;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }
    
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string Address { get; set; }
}
using System.IdentityModel.Tokens.Jwt;
 using System.Security.Claims;
 using System.Text;
 using Application.Abstractions.User;
 using Application.Contracts.Auth;
 using Infrastructure.Domain.User;
 using Microsoft.AspNetCore.Identity;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.IdentityModel.Tokens;
 
 namespace ColivingReservationsPlatform.Controllers;
 
 [Route("api/[controller]")]
 [ApiController]
 public class AuthController : ControllerBase
 {
     private readonly UserManager<User> _userManager;
     private readonly IConfiguration _configuration;
     private readonly RoleManager<IdentityRole<Guid>> _roleManager;
     private readonly IUserContextService _userContextService;
 
     public AuthController(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration, IUserContextService userContextService)
     {
         _userManager = userManager;
         _roleManager = roleManager;
         _configuration = configuration;
         _userContextService = userContextService;
     }
     
     [HttpGet("colivingOwners")]
     public async Task<ActionResult<UserResponseListItemDto[]>> Get()
     {
         var userRole = _userContextService.GetUserRole();

         try
         {
             if (userRole != "Administrator")
             {
                 return StatusCode(400, new { Error = "You do not have permission to get coliving owners." });
             }
             var role = await _roleManager.FindByNameAsync("ColivingOwner");
             var users = await _userManager.GetUsersInRoleAsync(role.Name);
             return Ok(users);
         }
         catch (KeyNotFoundException ex)
         {
             return NotFound(new { message = ex.Message });
         }
     }
 
     [HttpPost("login")]
     public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
     {
         var user = await _userManager.FindByNameAsync(model.Username);
         if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
         {
             return Unauthorized();
         }
         
         var roles = await _userManager.GetRolesAsync(user);
         
         var claims = new List<Claim>
         {
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Name, user.UserName!),
         };
         
         foreach (var role in roles)
         {
             claims.Add(new Claim(ClaimTypes.Role, role));
         }
         
         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
         
         var token = new JwtSecurityToken(
             issuer: null,
             audience: null,
             claims: claims,
             expires: DateTime.Now.AddDays(7),
             signingCredentials: creds);
         
         return Ok(new
         {
             Token = new JwtSecurityTokenHandler().WriteToken(token),
             Expiration = token.ValidTo,
             Roles = roles
         });
     }
 
     [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegistrationCreateDto model)
{
    try
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return StatusCode(500, new { Error = "A user with this username already exists." });
        }
        
        var user = new User
        {
            UserName = model.Username,
            FullName = model.Name + ' ' + model.Surname,
            Address = "",
            Name = model.Name,
            Surname = model.Surname,
            NormalizedUserName = model.Name + ' ' + model.Surname,
            Email = model.Email,
        };
        
        var roles = await _roleManager.Roles.ToListAsync(); // Materialize roles
        foreach (var role in roles)
        {
            Console.WriteLine(role.Name);
        }
        var roleExists = await _roleManager.RoleExistsAsync(model.Role);
        if (!roleExists)
        {
            return StatusCode(500, new { Error = "Specified role does not exist." });
        }


        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return StatusCode(500, new { Error = "User creation failed.", Details = result.Errors.Select(e => e.Description) });
        }
        
        await _userManager.AddToRoleAsync(user, model.Role.ToString());
        
        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, model.Role.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            Role = model.Role
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { Error = "An error occurred during user registration.", ExceptionMessage = ex.Message });
    }
}
}
using System.IdentityModel.Tokens.Jwt;
 using System.Security.Claims;
 using System.Text;
 using Application.Contracts.Auth;
 using Microsoft.AspNetCore.Identity;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.IdentityModel.Tokens;
 
 namespace ColivingReservationsPlatform.Controllers;
 
 [Route("api/[controller]")]
 [ApiController]
 public class AuthController : ControllerBase
 {
     private readonly UserManager<IdentityUser> _userManager;
     private readonly IConfiguration _configuration;
     private readonly RoleManager<IdentityRole> _roleManager;
 
     public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
     {
         _userManager = userManager;
         _roleManager = roleManager;
         _configuration = configuration;
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
             new Claim(ClaimTypes.NameIdentifier, user.Id),
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
        
        var user = new IdentityUser
        {
            UserName = model.Username,
            Email = model.Email
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return StatusCode(500, new { Error = "User creation failed.", Details = result.Errors.Select(e => e.Description) });
        }
        
        var roleExists = await _roleManager.RoleExistsAsync(model.Role.ToString());
        if (!roleExists)
        {
            return StatusCode(500, new { Error = "Specified role does not exist." });
        }
        
        await _userManager.AddToRoleAsync(user, model.Role.ToString());
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
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
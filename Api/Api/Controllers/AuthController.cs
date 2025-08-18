using Api.Data;
using Api.Models;
using Api.Models.Dtos;
using Api.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private string secretKey;

    public AuthController(ApplicationDbContext context, IConfiguration configuration,
        RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _configuration = configuration;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        _roleManager = roleManager;
        _userManager = userManager;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
    {
        var userFromDb = await _context.ApplicationUsers
            .FirstOrDefaultAsync(x => x.UserName == model.UserName);

        if (userFromDb != null)
            return BadRequest("Username already exists");

        if (model.Role.ToLower() != SD.Role_Admin.ToLower() &&
            model.Role.ToLower() != SD.Role_Instructor.ToLower() &&
            model.Role.ToLower() != SD.Role_Student.ToLower())
            return BadRequest("Invalid Role");

        var newUser = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.UserName,
            NormalizedEmail = model.UserName.ToUpper(),
            Name = model.Name,
        };

        var result = await _userManager.CreateAsync(newUser, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));

        if (!await _roleManager.RoleExistsAsync(SD.Role_Instructor))
            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Instructor));

        if (!await _roleManager.RoleExistsAsync(SD.Role_Student))
            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Student));

        await _userManager.AddToRoleAsync(newUser, model.Role);

        return Ok("Registration successful");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var userFromDb = await _context.ApplicationUsers
            .FirstOrDefaultAsync(x => x.UserName == model.UserName);

        if (userFromDb == null)
        {
            return BadRequest("Username or password is incorrect");
        }

        bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

        if (!isValid)
        {
            return BadRequest("Username or password is incorrect");
        }

        // Generate JWT token
        var roles = await _userManager.GetRolesAsync(userFromDb);
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(secretKey);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim("fullName", userFromDb.Name),
            new Claim("id", userFromDb.Id.ToString()),
            new Claim(ClaimTypes.Email, userFromDb.UserName),
            new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "User"),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        LoginResponseDto loginResponse = new()
        {
            Email = userFromDb.Email,
            Token = tokenHandler.WriteToken(token),
        };

        return Ok(loginResponse);
    }

}

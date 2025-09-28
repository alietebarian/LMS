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

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        private string sekretKey;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
            sekretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var userFromDb = await _context.ApplicationUsers
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == model.UserName.ToLower());

            if (userFromDb != null)
                return BadRequest("User already exists");

            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.UserName,
                NormalizedEmail = model.UserName.ToUpper(),
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
                return BadRequest("Error while registering");

            string[] roles = { SD.Role_Admin, SD.Role_Instructor, SD.Role_Student };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            string assignedRole = SD.Role_Student; // پیش‌فرض
            if (!string.IsNullOrWhiteSpace(model.Role))
            {
                // بررسی دقیق نقش انتخابی
                if (roles.Contains(model.Role, StringComparer.OrdinalIgnoreCase))
                    assignedRole = roles.First(r => r.Equals(model.Role, StringComparison.OrdinalIgnoreCase));
            }

            await _userManager.AddToRoleAsync(newUser, assignedRole);

            return Ok(new { message = "Registration successful", role = assignedRole });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            ApplicationUser userFromDb = await _context.ApplicationUsers
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == model.UserName.ToLower());

            if (userFromDb == null)
                return BadRequest("Username or password is incorrect");

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

            if (!isValid)
                return BadRequest("Username or password is incorrect");

            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(sekretKey);

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
}

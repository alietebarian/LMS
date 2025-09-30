using Api.Data;
using Api.Models;
using Api.Models.Dtos;
using Api.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public InstructorController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var instructor = await _context.Users
            .Include(u => u.CoursesTaught)
            .ToListAsync();

        if (!instructor.Any())
            return NotFound("Instructor not found");

        var result = instructor.Select(i => new InstructorDto
        {
            Id = i.Id,
            Name = i.Name,
            UserName = i.UserName,
            CoursesTaught = i.CoursesTaught
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title
            }).ToList()
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var instructor = await _context.Users
            .Include(u => u.CoursesTaught)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (instructor == null)
            return NotFound("Instructor not found");

        var result = new InstructorDto
        {
            Id = instructor.Id,
            Name = instructor.Name,
            UserName = instructor.UserName,
            CoursesTaught = instructor.CoursesTaught
            .Select(c => new CourseDto { Id = c.Id, Title = c.Title })
            .ToList()
        };

        return Ok(result);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInstructor(string id)
    {
        var instructor = await _userManager.FindByIdAsync(id);

        if (instructor == null)
            return BadRequest("There is no instructor with this id");

        var roles = await _userManager.GetRolesAsync(instructor);

        if (!roles.Contains(SD.Role_Instructor))
            return BadRequest("This user is not an instructor");

        var result = await _userManager.DeleteAsync(instructor);

        if (!result.Succeeded)
            return BadRequest("Error while deleting instructor");

        return Ok("Deleted successfully");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditInstructorInformation([FromBody] InstructorEditInformationDto model, string id)
    {
        var instructor = await _userManager.FindByIdAsync(id);

        if (instructor == null)
            return BadRequest("There is no instructor with this id");

        var roles = await _userManager.GetRolesAsync(instructor);

        if (!roles.Contains(SD.Role_Instructor))
            return BadRequest("This user is not an instructor");

        instructor.Name = model.Name;
        instructor.Email = model.Email;
        instructor.UserName = model.UserName;

        var result = await _userManager.UpdateAsync(instructor);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (!string.IsNullOrEmpty(model.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(instructor);
            var passResult = await _userManager.ResetPasswordAsync(instructor, token, model.Password);

            if (!passResult.Succeeded)
                return BadRequest(passResult.Errors);
        }

        return Ok("Instructor information updated successfully");
    }

    [HttpPost("addNewInstructor")]
    public async Task<IActionResult> CreateInstructor([FromBody] CreateInstructorDto model)
    {
        var userFromDb = await _context.ApplicationUsers
            .FirstOrDefaultAsync(x => x.UserName.ToLower() == model.UserName.ToLower());

        if (userFromDb != null)
            return BadRequest("this userName already exist");

        var newInstructor = new ApplicationUser
        {
            UserName = model.UserName,
            Name = model.Name,
            Email = model.Email,
        };

        var result = await _userManager.CreateAsync(newInstructor, model.Password);

        if (!result.Succeeded)
            return BadRequest("Error while registering");

        if (!await _roleManager.RoleExistsAsync(SD.Role_Instructor))
            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Instructor));

        await _userManager.AddToRoleAsync(newInstructor, SD.Role_Instructor);

        return Ok("Instructor added successfully");
    }

}

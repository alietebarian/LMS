using Api.Data;
using Api.Models;
using Api.Models.Dtos;
using Api.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _context.Courses
            .Include(x => x.Instructor)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                InstructorName = c.Instructor.Name,
                StructorId = c.InstrutorId
            })
            .ToListAsync();

        if (!courses.Any())
            return NotFound("there is no course");

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Instructor)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                InstructorName = c.Instructor.Name,
                StructorId = c.InstrutorId
            })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (course == null)
            return NotFound("there is no course with this id");

        return Ok(course);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
            return BadRequest("There is no course with this id");

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return Ok("Course deleted successfully");
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto model)
    {
        var instructor = await _userManager.FindByIdAsync(model.InstructorId);

        if (instructor == null)
            return BadRequest("instructor not found");

        var roles = await _userManager.GetRolesAsync(instructor);
        if (!roles.Contains(SD.Role_Instructor))
            return BadRequest("this user is not a instructor");

        var newCourse = new Course
        {
            Title = model.Title,
            InstrutorId = model.InstructorId,
        };

        _context.Courses.Add(newCourse);
        await _context.SaveChangesAsync();

        return Ok("course created");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCourseInformation([FromBody] EditCourseDto model ,int id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);

        if (course == null)
            return NotFound("There is no course with this id");

        course.Title = model.Title;

        await _context.SaveChangesAsync();

        var updatedCourse = new EditResponseDto
        {
            Title = course.Title
        };

        return Ok(updatedCourse);
    }
}

using Api.Data;
using Api.Models;
using Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssignmentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AssignmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{courseId}")]
    public async Task<IActionResult> GetAssignmentsByCourse(int courseId)
    {
        var assignmet = await _context.Assignments
            .Include(x => x.Course)
            .Where(a => a.CourseId == courseId)
            .Select(a => new AssignmentDto
            {
                Id = a.Id,
                Title = a.Title,
                CourseId = a.CourseId,
                CourseTitle = a.Course.Title,
            })
            .ToListAsync();

        if (!assignmet.Any())
            return NotFound($"No assignments found for course with ID {courseId}.");

        return Ok(assignmet);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssignmentByCourse([FromBody] CreateAssignmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var course = await _context.Courses.FindAsync(dto.CourseId);
        if (course == null)
            return NotFound($"Course with ID {dto.CourseId} not found.");

        var assignment = new Assignment
        {
            Title = dto.Title,
            CourseId = dto.CourseId,
        };

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();

        return Ok(assignment);
    }

    [HttpPut("{courseId}/{assignmentId}")]
    public async Task<IActionResult> EditAssignment(int courseId, int assignmentId, [FromBody] EditAssignmentDto dto)
    {
        var assignment = await _context.Assignments
            .FirstOrDefaultAsync(a => a.CourseId == courseId && a.Id == assignmentId);

        if (assignment == null)
            return NotFound($"No assignment found with ID {assignmentId} in Course {courseId}");

        assignment.Title = dto.Title;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Assignment updated successfully",
            updatedAssignment = new
            {
                assignment.Id,
                assignment.Title,
                assignment.CourseId
            }
        });
    }


}

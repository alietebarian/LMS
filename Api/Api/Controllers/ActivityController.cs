using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ActivityController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities()
    {
        var activities = await _context.Activitys.ToListAsync();

        return Ok(activities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivity(int id)
    {
        var activity = await _context.Activitys.SingleOrDefaultAsync(x => x.Id == id);

        if (activity == null)
            return NotFound("Not Found");
        if (id == 0)
            return BadRequest("Id can't be 0");

        return Ok(activity);
    }
}

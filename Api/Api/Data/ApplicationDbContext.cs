using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions opt) : base(opt)   
    {
        
    }

    public DbSet<Activity> Activitys { get; set; }
}

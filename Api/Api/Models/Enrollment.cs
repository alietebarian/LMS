namespace Api.Models;

public class Enrollment
{
    public int Id { get; set; }

    // FK به User
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    // FK به Course
    public int CourseId { get; set; }
    public Course Course { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}

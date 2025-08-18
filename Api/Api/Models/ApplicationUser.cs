using Microsoft.AspNetCore.Identity;

namespace Api.Models;

public class ApplicationUser : IdentityUser
{

    public string Name { get; set; }
    // Navigation Properties
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Course> CoursesTaught { get; set; } = new List<Course>();
    public ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();
    public ICollection<QuizSubmission> QuizSubmissions { get; set; } = new List<QuizSubmission>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

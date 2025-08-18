namespace Api.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }

    // مدرس
    public string InstrutorId { get; set; }
    public ApplicationUser Instructor { get; set; }

    // Navigations
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

namespace Api.Models;

public class Assignment
{
    public int Id { get; set; }
    public string Title { get; set; }

    // FK به Course
    public int CourseId { get; set; }
    public Course Course { get; set; }

    // Navigations
    public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
}

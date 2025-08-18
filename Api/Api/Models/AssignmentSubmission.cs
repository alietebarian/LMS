namespace Api.Models;

public class AssignmentSubmission
{
    public int Id { get; set; }
    public string Content { get; set; }

    // FK به Assignment
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; }

    // FK به Student (User)
    public string StudentId { get; set; }
    public ApplicationUser Student { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

namespace Api.Models;

public class QuizSubmission
{
    public int Id { get; set; }
    public string Answer { get; set; }

    // FK به Quiz
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }

    // FK به Student (User)
    public string StudentId { get; set; }
    public ApplicationUser Student { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

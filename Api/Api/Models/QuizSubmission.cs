namespace Api.Models;

public class QuizSubmission
{
    public int Id { get; set; }
    public string Answer { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }
}

namespace Api.Models;

public class QuizQuestion
{
    public int Id { get; set; }
    public string QuestionText { get; set; }

    // FK به Quiz
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }
}

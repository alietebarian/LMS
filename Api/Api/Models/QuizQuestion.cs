namespace Api.Models;

public class QuizQuestion
{
    public int Id { get; set; }
    public string QuestionText { get; set; }

    public string OptionA { get; set; }
    public string OptionB { get; set; }
    public string OptionC { get; set; }
    public string OptionD { get; set; }

    // ذخیره گزینه صحیح: "A", "B", "C", "D"
    public string CorrectOption { get; set; }

    // FK به Quiz
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; }
}

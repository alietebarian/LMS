namespace Api.Models;

public class Quiz
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; }

    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();

    // نگه‌داری Submissions
    public ICollection<QuizSubmission> Submissions { get; set; } = new List<QuizSubmission>();


}

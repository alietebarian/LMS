namespace Api.Models;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; }

    // FK به Course
    public int CourseId { get; set; }
    public Course Course { get; set; }

    // Navigations
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public ICollection<QuizSubmission> Submissions { get; set; } = new List<QuizSubmission>();
}

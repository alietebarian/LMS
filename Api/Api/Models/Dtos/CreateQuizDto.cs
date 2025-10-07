namespace Api.Models.Dtos;

public class CreateQuizDto
{
    public string Title { get; set; }
    //public int CourseId { get; set; }
    public List<CreateQuizQuestionDto> Questions { get; set; } = new();
}

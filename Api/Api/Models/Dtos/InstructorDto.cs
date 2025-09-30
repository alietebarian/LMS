namespace Api.Models.Dtos;

public class InstructorDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public List<CourseDto> CoursesTaught { get; set; }
}

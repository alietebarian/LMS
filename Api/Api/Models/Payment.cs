namespace Api.Models;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    // FK به User
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    // FK به Course
    public int CourseId { get; set; }
    public Course Course { get; set; }
}

using Api.Data;
using Api.Models;
using Api.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuizController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{courseId}")]   //گرفتن quiz های یک دوره که با پاس دادن آیدی آن دوره سوالات را بگیرم
    public async Task<IActionResult> GetQuizByCourseId(int courseId)
    {
        var quizDto = await _context.QuizQuestions
            .Where(q => q.Quiz.CourseId == courseId)
            .Select(x => new QuizDto
            {
                QuistionTest = x.QuestionText,
                OptionA = x.OptionA,
                OptionB = x.OptionB,
                OptionC = x.OptionC,
                OptionD = x.OptionD
            })
            .ToListAsync();

        if (quizDto == null)
            return BadRequest("there is no question for this course");

        return Ok(quizDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles ="Admin,Instructor")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var quiz = await _context.Quizzes
            .Include(x => x.Questions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (quiz == null)
            return NotFound(new { message = "Quiz یافت نشد." });

        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();

        return Ok("Deleted");
    }

    [HttpPut("{id}")]
    [Authorize(Roles ="Admin,Instructor")]
    public async Task<IActionResult> EditQuiz(int id, [FromBody]  QuizDto model)
    {
        var quiz = await _context.QuizQuestions
            .FirstOrDefaultAsync(x => x.Quiz.Id == id);

        if (quiz == null)
            return NotFound("QuizQuestion not found");

        quiz.QuestionText = model.QuistionTest;
        quiz.OptionA = model.OptionA;
        quiz.OptionB = model.OptionB;
        quiz.OptionC = model.OptionC;
        quiz.OptionD = model.OptionD;

        await _context.SaveChangesAsync();

        return Ok("Quiz updated successfully");
    }

    [HttpPost("{courseId}")]
    [Authorize(Roles ="Admin,Instructor")]
    public async Task<IActionResult> CreateQuiz(int courseId,[FromBody] CreateQuizDto dto)
    {
        var quiz = new Quiz
        {
            CourseId = courseId,
            Questions = dto.Questions.Select(q => new QuizQuestion
            {
                QuestionText = q.QuestionText,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                CorrectOption = q.CorrectAnswer,
            }).ToList(),
        };

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();

        return Ok("Created successfully");
    }
}

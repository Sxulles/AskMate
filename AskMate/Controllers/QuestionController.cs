using AskMate.Model;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AskMate.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    // Get all questions
    [HttpGet]
    public IActionResult GetAll()
    {
        var repository = new QuestionRepository(new NpgsqlConnection(ConnectionData.connectionString));
        return Ok(repository.GetAll("DESC", "submission_time"));
    }
    
    // Get a question and the answers
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var repository = new QuestionRepository(new NpgsqlConnection(ConnectionData.connectionString));
        
        return Ok(repository.GetById(id));
    }
    
    // Insert a new Question into database
    [HttpPost()]
    public IActionResult CreateQuestion(Question question)
    {
        var repository = new QuestionRepository(new NpgsqlConnection(ConnectionData.connectionString));

        return Ok(repository.CreateQuestion(question));
    }
}
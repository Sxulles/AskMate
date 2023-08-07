using AskMate.Model;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AskMate.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var repository = new QuestionRepository(new NpgsqlConnection(ConnectionData.connectionString));

        return Ok(repository.GetAll());
    }
}
using AskMate.Model;
using AskMate.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace AskMate.Controllers;

[ApiController]
[Route("[controller]")]
public class AnswerController : ControllerBase
{
    // Create an answer for a specific question by id
    [HttpPost]
    public IActionResult CreateAnswer(Answer answer)
    {
        var repository = new AnswerRepository(new NpgsqlConnection(ConnectionData.connectionString));

        return Ok(repository.CreateAnswer(answer));
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var repository = new AnswerRepository(new NpgsqlConnection(ConnectionData.connectionString));
        repository.Delete(id);

        return Ok();
    }
}

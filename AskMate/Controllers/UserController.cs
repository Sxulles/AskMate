using AskMate.Model;
using AskMate.Model.Repositories;
using Npgsql;

namespace AskMate.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    // Insert a new user to the database
    [HttpPost()]
    public IActionResult CreateUser(User user)
    {
        var repository = new UserRepository(new NpgsqlConnection(ConnectionData.connectionString));

        return Ok(repository.CreateUser(user));
    }
}
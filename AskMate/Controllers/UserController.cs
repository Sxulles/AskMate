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
    [HttpPost("/User/create/{username}, {email}, {password}")]
    public IActionResult CreateUser(string  username, string  email, string password)
    {
        var repository = new UserRepository(new NpgsqlConnection(ConnectionData.connectionString));

        return Ok(repository.CreateUser(username, email, password));
    }

    [HttpPost("/User/login/{username}, {password}")]
    public IActionResult LogUser(string username, string password)
    {
        var repository = new UserRepository(new NpgsqlConnection(ConnectionData.connectionString));

        return Ok(repository.AuthUser(username, password));
    }
}
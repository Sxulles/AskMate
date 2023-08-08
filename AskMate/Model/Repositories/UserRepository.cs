using System.Data;
using Npgsql;

namespace AskMate.Model.Repositories;

public class UserRepository
{
    private readonly NpgsqlConnection _connection;

    public UserRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    // Create a new User
    public int CreateUser(string username, string email, string password)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter(
            "INSERT INTO users (username, email, password, registration_time) VALUES (:username, :email, :password, :registration_time) RETURNING id",
            _connection
        );
        adapter.SelectCommand?.Parameters.AddWithValue(":username", username);
        adapter.SelectCommand?.Parameters.AddWithValue(":email", email);
        adapter.SelectCommand?.Parameters.AddWithValue(":password", password);
        adapter.SelectCommand?.Parameters.AddWithValue(":registration_time", DateTime.Now);

        var lastInsertId = (int)adapter.SelectCommand?.ExecuteScalar();
        _connection.Close();

        return lastInsertId;
    }

    public string AuthUser(string username, string password)
    {
        _connection.Open();
        
        using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = :username AND password = :password",_connection))
        {
            command.Parameters.AddWithValue(":username", username);
            command.Parameters.AddWithValue(":password", password);
            
            int userCount = Convert.ToInt32(command.ExecuteScalar());
            
            if (userCount > 0)
            {
                return "Successfully logged in!";
            }
        }
        return "Invalid user!";
    }
}
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
    public int CreateUser(User user)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter(
            "INSERT INTO users (username, email, password, registration_time) VALUES (:username, :email, :password, :registration_time) RETURNING id",
            _connection
        );
        adapter.SelectCommand?.Parameters.AddWithValue(":username", user.Username);
        adapter.SelectCommand?.Parameters.AddWithValue(":email", user.Email);
        adapter.SelectCommand?.Parameters.AddWithValue(":password", user.Password);
        adapter.SelectCommand?.Parameters.AddWithValue(":registration_time", DateTime.Now);

        var lastInsertId = (int)adapter.SelectCommand?.ExecuteScalar();
        _connection.Close();

        return lastInsertId;
    }
}
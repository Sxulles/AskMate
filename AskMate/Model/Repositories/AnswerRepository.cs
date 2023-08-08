using System.Data;
using Microsoft.AspNetCore.Components.Web;
using Npgsql;

namespace AskMate.Model.Repositories;

public class AnswerRepository
{
    private readonly NpgsqlConnection _connection;

    public AnswerRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public int? CreateAnswer(string message, int questionId)
    {
        _connection.Open();
        var query = new NpgsqlDataAdapter("SELECT * FROM questions WHERE id = :question_id", _connection);
        query.SelectCommand?.Parameters.AddWithValue(":question_id", questionId);
        DataTable queryResult = new DataTable();
    
        
        query.Fill(queryResult);
        int? lastInsertId = null;
        
        
        if (queryResult.Rows.Count > 0)
        {
            var adapter = new NpgsqlDataAdapter(
                "INSERT INTO answers (message, question_id, submission_time) VALUES (:message, :question_id, :submission_time) RETURNING id",
                _connection
            );
            adapter.SelectCommand?.Parameters.AddWithValue(":message", message);
            adapter.SelectCommand?.Parameters.AddWithValue(":question_id", questionId);
            adapter.SelectCommand?.Parameters.AddWithValue(":submission_time", DateTime.Now);

            lastInsertId = (int)adapter.SelectCommand?.ExecuteScalar();
        }
        _connection.Close();
        
        return lastInsertId;
    }
    public void Delete(int id)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter(
            "DELETE FROM answers WHERE id = :id",
            _connection
        );
        adapter.SelectCommand?.Parameters.AddWithValue(":id", id);

        adapter.SelectCommand?.ExecuteNonQuery();
        _connection.Close();
    }
    
    //Accept answer
    public void AcceptAnswer(int id)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter(
            "SELECT question_id FROM answers WHERE id = :id",
            _connection
        );
        adapter.SelectCommand?.Parameters.AddWithValue(":id", id);
        
        var dataTable = new DataTable();
        adapter.Fill(dataTable);
        int questionId = 0;

        foreach (DataRow row in dataTable.Rows)
        {
            questionId = (int)row["question_id"];
        }
        
        var query = new NpgsqlDataAdapter($"UPDATE questions SET accepted_answer_id = {id} WHERE id = {questionId}", _connection);
        query.SelectCommand?.ExecuteNonQuery();
        
        _connection.Close();
    }
}
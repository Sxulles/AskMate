﻿using System.Data;
using Npgsql;

namespace AskMate.Model.Repositories;

public class AnswerRepository
{
    private readonly NpgsqlConnection _connection;

    public AnswerRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public int? CreateAnswer(Answer answer)
    {
        _connection.Open();
        var query = new NpgsqlDataAdapter("SELECT * FROM questions WHERE id = :question_id", _connection);
        query.SelectCommand?.Parameters.AddWithValue(":question_id", answer.QuestionId);
        DataTable queryResult = new DataTable();
    
        
        query.Fill(queryResult);
        int? lastInsertId = null;
        
        
        if (queryResult.Rows.Count > 0)
        {
            var adapter = new NpgsqlDataAdapter(
                "INSERT INTO answers (message, question_id, submission_time) VALUES (:message, :question_id, :submission_time) RETURNING id",
                _connection
            );
            adapter.SelectCommand?.Parameters.AddWithValue(":message", answer.Message);
            adapter.SelectCommand?.Parameters.AddWithValue(":question_id", answer.QuestionId);
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
}
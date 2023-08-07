using System.Data;
using Npgsql;

namespace AskMate.Model.Repositories;

public class AnswerRepository
{
    private readonly NpgsqlConnection _connection;

    public AnswerRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public IEnumerable<Answer> GetAll()
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter("SELECT * FROM answers", _connection);

        var dataSet = new DataSet();
        adapter.Fill(dataSet);
        var table = dataSet.Tables[0];

        foreach (DataRow row in table.Rows)
        {
            yield return new Answer
            {
                Id = (int)row["id"],
                Message = (string)row["title"],
                QuestionId = (int)row["description"],
                SubmissionTime = (DateTime)row["submission_time"]
            };
        }
        
        _connection.Close();
    }
}
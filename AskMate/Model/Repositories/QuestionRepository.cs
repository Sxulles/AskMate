using System.Data;

namespace AskMate.Model;
using Npgsql;

public class QuestionRepository
{
    private readonly NpgsqlConnection _connection;

    public QuestionRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public IEnumerable<Question> GetAll(string order, string column)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter("SELECT * FROM questions", _connection);

        var dataSet = new DataSet();
        adapter.Fill(dataSet);
        var table = dataSet.Tables[0];
        
        table.DefaultView.Sort = $"{column} {order}"; // Sort by Submission Time

        foreach (DataRow row in table.Rows)
        {
            yield return new Question
            {
                Id = (int)row["id"],
                Title = (string)row["title"],
                Description = (string)row["description"],
                SubmissionTime = (DateTime)row["submission_time"]
            };
        }
        
        _connection.Close();
    }
}
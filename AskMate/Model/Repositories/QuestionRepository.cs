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

    public (Question, List<Answer>) GetById(int id)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter("SELECT q.id AS question_id, q.title, q.description, q.submission_time AS question_submission_time, a.id AS answer_id, a.message, a.submission_time AS answer_submission_time, a.question_id FROM questions q JOIN answers a ON q.id = a.question_id WHERE q.id = :id ", _connection);
        adapter.SelectCommand?.Parameters.AddWithValue(":id", id);

        var dataSet = new DataSet();
        adapter.Fill(dataSet);
        var table = dataSet.Tables[0];
        Question question;
        List<Answer> answers = new List<Answer>();
        (Question a, List<Answer> b) returnValue = (null, null);
        
        if (table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];
            question = new Question()
            {
                Id = (int)row["question_id"],
                Title = (string)row["title"],
                Description = (string)row["description"],
                SubmissionTime = (DateTime)row["question_submission_time"]
            };
            for (int i = 1; i < table.Rows.Count; i++)
            {
                answers.Add(new Answer()
                {
                    Id = (int)table.Rows[i]["answer_id"],
                    Message = (string)table.Rows[i]["message"],
                    QuestionId = (int)table.Rows[i]["question_id"],
                    SubmissionTime = (DateTime)table.Rows[i]["answer_submission_time"]
                });
            }
            returnValue.b = answers;
            returnValue.a = question;
        }
        return returnValue;
    }
    
}
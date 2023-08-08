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
    
    // Get all questions
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

    // Get a question and the answers
    public QuestionAnswersPair? GetById(int id)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter("SELECT q.id AS question_id, q.title, q.description, q.submission_time AS question_submission_time, a.id AS answer_id, a.message, a.submission_time AS answer_submission_time, a.question_id FROM questions q JOIN answers a ON q.id = a.question_id WHERE q.id = :id ", _connection);
        adapter.SelectCommand?.Parameters.AddWithValue(":id", id);

        var dataSet = new DataSet();
        adapter.Fill(dataSet);
        var table = dataSet.Tables[0];
        Question? question;
        List<Answer> answers = new List<Answer>();
        
        if (table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];
            question = new Question
            {
                Id = (int)row["question_id"],
                Title = (string)row["title"],
                Description = (string)row["description"],
                SubmissionTime = (DateTime)row["question_submission_time"]
            };
            foreach (DataRow newRow in table.Rows)
            {
                answers.Add(new Answer()
                {
                    Id = (int)newRow["answer_id"],
                    Message = (string)newRow["message"],
                    QuestionId = (int)newRow["question_id"],
                    SubmissionTime = (DateTime)newRow["answer_submission_time"]
                });
            }
            return new QuestionAnswersPair
            {
                Question = question, 
                Answers = answers
            };
        }
        return null;
    }
    
    // Create a new Question
    public int CreateQuestion(string question, string description)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter(
            "INSERT INTO questions (title, description, submission_time) VALUES (:title, :description, :submission_time) RETURNING id",
            _connection
        );
        adapter.SelectCommand?.Parameters.AddWithValue(":title", question);
        adapter.SelectCommand?.Parameters.AddWithValue(":description", description);
        adapter.SelectCommand?.Parameters.AddWithValue(":submission_time", DateTime.Now);

        var lastInsertId = (int)adapter.SelectCommand?.ExecuteScalar();
        _connection.Close();

        return lastInsertId;
    }
    //DELETE A QUESTION
    public void Delete(int id)
    {
        _connection.Open();
        var adapter = new NpgsqlDataAdapter(
            "DELETE FROM questions WHERE id = :id",
            _connection
        );
        adapter.SelectCommand?.Parameters.AddWithValue(":id", id);

        adapter.SelectCommand?.ExecuteNonQuery();
        _connection.Close();
    }
}
namespace AskMate.Model;

public class Question
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public DateTime submission_time { get; set; }
}
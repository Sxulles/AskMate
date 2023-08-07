namespace AskMate.Model;

public static class ConnectionData
{
    private static string? DB_HOST = Environment.GetEnvironmentVariable("DATABASE_HOST");
    private static string? DB_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME");
    private static string? DB_PORT = Environment.GetEnvironmentVariable("DATABASE_PORT");
    private static string? DB_USER = Environment.GetEnvironmentVariable("DATABASE_USER");
    private static string? DB_PASSWORD = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

    public static readonly string connectionString = $"Server={DB_HOST};Port={DB_PORT};User Id={DB_USER};Password={DB_PASSWORD};Database={DB_NAME}";
    
    
}
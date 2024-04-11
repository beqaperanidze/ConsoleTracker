using Spectre.Console;

namespace ConsoleTracker.dir;

using System.Data.SQLite;

public static class DatabaseManager
{
    public static void RunCodingSessions(string connectionString)
    {
        while (true)
        {
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1 - to add a new Session");
            Console.WriteLine("2 - to view all the Session");
            Console.WriteLine("3 - to update a Session");
            Console.WriteLine("4 - to delete a Session");
            Console.WriteLine("5 - EXIT");
            Console.WriteLine();
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddSession(connectionString);
                    break;
                case "2":
                    ViewAll(connectionString);
                    break;
                case "3":
                    UpdateSession(connectionString);
                    break;
                case "4":
                    DeleteSession(connectionString);
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
            }
        }
    }


    private static void CreateDb(string connectionString)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();
        const string query =
            "CREATE TABLE IF NOT EXISTS Sessions (Id INTEGER PRIMARY KEY, StartTime TEXT, FinishTime TEXT, Duration TEXT)";
        var command = new SQLiteCommand(query, connection);
        command.ExecuteNonQuery();
    }

    private static void AddSession(string connectionString)
    {
        var codingSession = new CodingSession();
        AnsiConsole.Write("Please enter your staring time: ");
        codingSession.StartTime = TimeSpan.Parse(Console.ReadLine() ?? string.Empty);
        AnsiConsole.Write("Please enter your finishing time: ");
        codingSession.FinishTime = TimeSpan.Parse(Console.ReadLine() ?? string.Empty);
        codingSession.Duration = codingSession.FinishTime.Subtract(codingSession.StartTime);
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();
        const string query =
            "INSERT INTO Sessions (StartTime, FinishTime, Duration) VALUES (@StartTime, @FinishTime, @Duration)";
        var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@StartTime", codingSession.StartTime.ToString());
        command.Parameters.AddWithValue("@FinishTime", codingSession.FinishTime.ToString());
        command.Parameters.AddWithValue("@Duration", codingSession.Duration.ToString());
        command.ExecuteNonQuery();
        AnsiConsole.WriteLine("\nYour session has been added successfully");
    }

    private static void ViewAll(string connectionString)
    {
        using var connection = new SQLiteConnection(connectionString);
        connection.Open();
        const string query = "SELECT * FROM Sessions";
        var command = new SQLiteCommand(query, connection);
        var reader = command.ExecuteReader();
        AnsiConsole.WriteLine("\nId\tStart time\tFinish time\tDuration");
        while (reader.Read())
        {
            AnsiConsole.WriteLine(
                $"{reader["Id"]}\t{reader["StartTime"]}\t{reader["FinishTime"]}\t{reader["Duration"]}\t");
        }

        AnsiConsole.WriteLine("\n");
    }

    private static void UpdateSession(string connectionString)
    {
        var codingSession = new CodingSession();
        AnsiConsole.Write("Enter the id of a session, which you want to update: ");
        codingSession.Id = int.Parse(Console.ReadLine() ?? string.Empty);
        AnsiConsole.Write("Enter a new starting time: ");
        codingSession.StartTime = TimeSpan.Parse(Console.ReadLine() ?? string.Empty);
        AnsiConsole.Write("Enter a new finishing time: ");
        codingSession.FinishTime = TimeSpan.Parse(Console.ReadLine() ?? string.Empty);
        codingSession.Duration = codingSession.FinishTime.Subtract(codingSession.StartTime);

        using var connection = new SQLiteConnection(connectionString);
        connection.Open();
        const string query =
            "UPDATE Sessions SET StartTime = @StartTime, FinishTime = @FinishTime, Duration = @Duration WHERE Id = @Id";
        var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@StartTime", codingSession.StartTime);
        command.Parameters.AddWithValue("@FinishTime", codingSession.FinishTime);
        command.Parameters.AddWithValue("@Duration", codingSession.Duration);
        command.Parameters.AddWithValue("@Id", codingSession.Id);
        var rowsUpdated = command.ExecuteNonQuery();

        Console.WriteLine(rowsUpdated > 0 ? "Session updated successfully." : "Session not found.");
    }

    private static void DeleteSession(string connectionString)
    {
        AnsiConsole.Write("Enter the id of a session, which you want to delete: ");
        var id = int.Parse(Console.ReadLine() ?? string.Empty);

        using var connection = new SQLiteConnection(connectionString);
        connection.Open();
        const string query = "DELETE FROM Sessions WHERE ID = @Id";
        var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        var rowsDeleted = command.ExecuteNonQuery();

        Console.WriteLine(rowsDeleted > 0 ? "Session has been deleted." : "Session not found.");
    }
}
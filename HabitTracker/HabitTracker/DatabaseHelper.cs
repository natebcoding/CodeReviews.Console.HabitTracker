using Microsoft.Data.Sqlite;

namespace HabitTracker;

public class DatabaseHelper
{

    public void StartConnection()
    {
        string connectionString = "DataSource=habitTracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = 
                @"CREATE TABLE IF NOT EXISTS Habits(
                    habitId INTEGER PRIMARY KEY AUTOINCREMENT, habitName TEXT, habitQuantity INTEGER, date TEXT)";

            tableCommand.ExecuteNonQuery();
            
        }

    }

    
    
    // Methods (Just using basic for now, need to update to DB methods)

    public string CreateHabit()
    {
        
        Console.WriteLine("Enter the Habit Name: ");
        string? habitName = Console.ReadLine();
        Console.WriteLine("Enter the Habit Quantity: ");
        int habitQuantity = int.Parse(Console.ReadLine());
        Console.WriteLine("""Date for the habit? Enter "today" for today, or another date.""");
        string? habitDate = Console.ReadLine();

        var habit = new Dictionary<string, object>
        {
            { "habitName", habitName },
            { "habitQuantity", habitQuantity },
            { "date", habitDate },
        };
        
        
        string createQuery = @"INSERT INTO Habits (habitName, habitQuantity, date) VALUES ($habitName, $habitQuantity, $habitDate)";
        
        Execute(createQuery, habit);
        
        return "Habit created";
    }

    public void Execute(string sql, Dictionary<string, object> parameters)
    {
        string connectionString = "DataSource=habitTracker.db";
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = sql;

            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
            

            command.ExecuteNonQuery();
        }
    }

    public string UpdateHabit()
    {
        return "Habit Updated";
    }
    
}
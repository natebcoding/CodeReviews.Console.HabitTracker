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

            connection.Close();
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
        Habit habit = new Habit(1, habitName ?? "Placeholder Name", habitQuantity, habitDate ?? "Placeholder Date");
        
        string connectionString = "DataSource=habitTracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = 
                @"INSERT INTO Habits VALUES ($habitName, $habitQuantity, $habitDate)";

            tableCommand.Parameters.AddWithValue("$habitName", habitName);
            tableCommand.Parameters.AddWithValue("$habitQuantity", habitQuantity);
            tableCommand.Parameters.AddWithValue("$habitDate", habitDate);

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }
        
        return "Habit created";
    }

    public string UpdateHabit()
    {
        return "Habit Updated";
    }
    
}
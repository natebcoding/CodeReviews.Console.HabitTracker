using System.Globalization;
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
    
    public void ExecuteNoParams(string sql)
    {
        string connectionString = "DataSource=habitTracker.db";
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = sql;
            

            command.ExecuteNonQuery();
        }
    }
    
    // CRUD Methods
    
    public string CreateHabit()
    {
        
        Console.WriteLine("Enter the Habit Name: ");
        string? habitName = Console.ReadLine();
        Console.WriteLine("Enter the Habit Quantity: ");
        int habitQuantity = 0;

        if (int.TryParse(Console.ReadLine(), out int habitQuantityResult))
        {
            habitQuantity = habitQuantityResult;
            
        }
        Console.WriteLine("""Date for the habit? Enter "today" for today, or another date. (Format: MM/dd/yyyy)""");
        string? habitDate = Console.ReadLine();
        string dateFormat = "MM/dd/yyyy";

        if (habitDate == "today")
        {
            habitDate = DateTime.Now.ToString("MM/dd/yyyy");
        }
        else
        {
            DateTime? validDate = CheckIfValidDate(habitDate);

            while (validDate == null)
            {
                Console.WriteLine("Enter a valid date (MM/dd/yyyy)");
                habitDate = Console.ReadLine();
                validDate = CheckIfValidDate(habitDate);

            }
            
        }

        var habit = new Dictionary<string, object>
        { // input variables are transformed to the placeholder ones for SQL query -- habitDate -> $date
            { "$habitName", habitName },
            { "$habitQuantity", habitQuantity },
            { "$date", habitDate },
        };
        
        
        string createQuery = @"INSERT INTO Habits (habitName, habitQuantity, date) VALUES ($habitName, $habitQuantity, $date)"; // The $ values must match the dictionary names $date -> $date here
        
        Execute(createQuery, habit);
        
        return "Habit created";
    }

    public string UpdateHabit()
    {
        string updateQuery = "";
        Console.WriteLine("Enter the ID of the habit you would like to update: ");
        int targetHabitId = 0;
        if (int.TryParse(Console.ReadLine(), out int idResult))
        {
            targetHabitId = idResult;
        }

        Console.WriteLine("What would you like to update on this habit?");
        Console.WriteLine("1. Habit Name");
        Console.WriteLine("2. Habit Quantity");
        Console.WriteLine("3. Habit Date");
        string? userInp = Console.ReadLine();

        switch (userInp)
        {
            case "1": // Update Habit Name
                Console.WriteLine("Enter the new habit name: ");
                string? newHabitName = Console.ReadLine();
                updateQuery = @"UPDATE Habits SET habitName = $newHabitName WHERE habitId = $targetHabitId";
                var updateNameParams = new Dictionary<string, object>
                    { { "$targetHabitId", targetHabitId }, { "$newHabitName", newHabitName ?? "Default Placeholder" } };
                Execute(updateQuery, updateNameParams);
                break;
            case "2": // Update Habit Quantity
                Console.WriteLine("Enter the new habit quantity: ");
                int newHabitQuantity = 0;
                if (int.TryParse(Console.ReadLine(), out int newHabitQuantityResult)) // Do null check, output as result
                {
                    newHabitQuantity = newHabitQuantityResult;
                }
                updateQuery = @"UPDATE Habits SET habitQuantity = $newHabitQuantity WHERE habitId = $targetHabitId";
                var updateQuantityParams = new Dictionary<string, object>
                    { { "$targetHabitId", targetHabitId }, { "$newHabitQuantity", newHabitQuantity} };
                Execute(updateQuery, updateQuantityParams);
                break;
            case "3": // Update Habit Date
                Console.WriteLine("What is the new date (MM/dd/yy)?");
                string newHabitDate = Console.ReadLine();
                updateQuery = @"UPDATE Habits SET date = $newHabitDate WHERE habitId = $targetHabitId";
                var updateDateParams = new Dictionary<string, object>
                    { { "$targetHabitId", targetHabitId }, { "$newHabitDate", newHabitDate} };
                Execute(updateQuery, updateDateParams);
                break;
            default:
                Console.WriteLine("Incorrect option");
                break;
        }
        
        return "Habit Updated";
    }

    public string DeleteHabit()
    {
        string query = @"DELETE FROM Habits WHERE habitId = $targetHabitId";
        
        Console.WriteLine("Enter the ID of the habit you would like to delete: ");
        int targetHabitId = 0;
        if (int.TryParse(Console.ReadLine(), out int targetHabitIdresult))
        {
            targetHabitId = targetHabitIdresult;
            
        }
        var deleteParams = new Dictionary<string, object> { { "$targetHabitId", targetHabitId } };
        Execute(query, deleteParams);

        return "Habit Deleted";
    }
    
    public void ExecuteReader(string sql)
    {
        string connectionString = "DataSource=habitTracker.db";
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = new SqliteCommand(sql, connection))
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int habitId = reader.GetInt32(0);
                        string habitName = reader.GetString(1);
                        int habitQuantity = Convert.ToInt32(reader["habitQuantity"]);
                        string date = reader.GetString(3);

                        Console.WriteLine($"ID: {habitId}, Habit Name: {habitName}, Quantity: {habitQuantity}, Date: {date}");
                    }
                }
            }
            
        }
        
    }

    public DateTime? CheckIfValidDate(string dateInput)
    {
        string dateFormat = "MM/dd/yyyy";
        bool isValid = DateTime.TryParseExact(dateInput, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out DateTime parsedDate);

        if (isValid)
        {
            return parsedDate;
        }
        
        return null;
    }
    
    public void GetAllHabits()
    {
        string query = @"SELECT * FROM Habits";
        
        ExecuteReader(query);
    }
    
}
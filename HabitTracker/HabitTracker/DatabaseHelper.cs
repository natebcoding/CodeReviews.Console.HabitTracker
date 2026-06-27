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
                    habitId INTEGER PRIMARY KEY AUTOINCREMENT, habitName TEXT, habitUnit TEXT, habitQuantity INTEGER, date TEXT)";
            

            tableCommand.ExecuteNonQuery();
            
            string existsQuery = @"SELECT COUNT(*) FROM Habits";
            var count = ExecuteScalarNoParams(existsQuery);
            if (count == 0)
            {
                var seedData = new Dictionary<string, object>
                { // input variables are transformed to the placeholder ones for SQL query -- habitDate -> $date
                    { "$habitName", "Drink Water"},
                    { "$habitUnit", "glasses"},
                    { "$habitQuantity", 5 },
                    { "$date", "06/24/2026" },
                };
        
        
                string createQuery = @"INSERT INTO Habits (habitName, habitUnit, habitQuantity, date) VALUES ($habitName, $habitUnit, $habitQuantity, $date)"; 
        
                Execute(createQuery, seedData);
            
            }
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
    public int ExecuteScalar(string sql, Dictionary<string, object> parameters)
    {
        int count = 0;
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

            object result = command.ExecuteScalar();
            count = result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        return count;
    }
    public int ExecuteScalarNoParams(string sql)
    {
        int count = 0;
        string connectionString = "DataSource=habitTracker.db";
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = sql;

            object result = command.ExecuteScalar();
            count = result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        return count;
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
        string? habitName = string.Empty;
        while (string.IsNullOrEmpty(habitName))
        {
            Console.WriteLine("Enter the Habit Name: ");
            habitName = Console.ReadLine();
            
        }

        string? habitUnit = string.Empty;
        while (string.IsNullOrEmpty(habitUnit))
        {
            Console.WriteLine("Enter the unit you are tracking (miles, reps, etc):");
            habitUnit = Console.ReadLine();
        }
        
        Console.WriteLine("Enter the Habit Quantity: ");
        int habitQuantity = 0;

        while (!int.TryParse(Console.ReadLine(), out habitQuantity))
        {
            Console.WriteLine("Habit Quantity must be greater than 0.");
            
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
            { "$habitUnit", habitUnit},
            { "$habitQuantity", habitQuantity },
            { "$date", habitDate },
        };
        
        
        string createQuery = @"INSERT INTO Habits (habitName, habitUnit, habitQuantity, date) VALUES ($habitName, $habitUnit, $habitQuantity, $date)"; // The $ values must match the dictionary names $date -> $date here
        
        Execute(createQuery, habit);
        
        return "Habit created";
    }

    public string UpdateHabit()
    {
        string updateQuery = "";
        
        Console.WriteLine("Enter the ID of the habit you would like to update: ");
        int targetHabitId = 0;
        if (int.TryParse(Console.ReadLine(), out int targetHabitIdresult))
        {
            targetHabitId = targetHabitIdresult;
            
        }
        
        string existsQuery = @"SELECT COUNT(*) FROM Habits WHERE habitId = $targetHabitId";
        var deleteParams = new Dictionary<string, object> { { "$targetHabitId", targetHabitId } };
        var count = ExecuteScalar(existsQuery, deleteParams);
        if (count == 1)
        {
            Console.WriteLine("What would you like to update on this habit?");
            Console.WriteLine("1. Habit Name");
            Console.WriteLine("2. Habit Unit");
            Console.WriteLine("3. Habit Quantity");
            Console.WriteLine("4. Habit Date");
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
                case "2": // Update Habit Unit
                    Console.WriteLine("Enter the new Habit unit ");
                    string? newHabitUnit = Console.ReadLine();
                    updateQuery = @"UPDATE Habits SET habitUnit = $newHabitUnit WHERE habitId = $targetHabitId";
                    var updateHabitUnitParams = new Dictionary<string, object>
                        { { "$targetHabitId", targetHabitId }, { "$newHabitUnit", newHabitUnit ?? "Default Placeholder" } };
                    Execute(updateQuery, updateHabitUnitParams);
                    break;
                case "3": // Update Habit Quantity
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
                case "4": // Update Habit Date
                    Console.WriteLine("What is the new date (MM/dd/yyyy) or today?");
                    string? newHabitDate = Console.ReadLine();
                    if (newHabitDate == "today")
                    {
                        newHabitDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        DateTime? validDate = CheckIfValidDate(newHabitDate);

                        while (validDate == null)
                        {
                            Console.WriteLine("Enter a valid date (MM/dd/yyyy)");
                            newHabitDate = Console.ReadLine();
                            validDate = CheckIfValidDate(newHabitDate);

                        }
                
                    }
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
        
        return "No Habits with this ID found";
        
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
        
        string existsQuery = @"SELECT COUNT(*) FROM Habits WHERE habitId = $targetHabitId";
        var deleteParams = new Dictionary<string, object> { { "$targetHabitId", targetHabitId } };
        var count = ExecuteScalar(existsQuery, deleteParams);
        if (count == 1)
        {
            Execute(query, deleteParams);
            return "Habit Deleted";
            
        }

        return "No Habits with this ID found";
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
                        string habitUnit = reader.GetString(2);
                        int habitQuantity = Convert.ToInt32(reader["habitQuantity"]);
                        string date = reader.GetString(4);
                        

                        Console.WriteLine($"ID: {habitId}, Habit Name: {habitName}, Quantity: {habitQuantity} {habitUnit}, Date: {date}");
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
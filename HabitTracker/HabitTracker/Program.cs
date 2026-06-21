namespace HabitTracker;

// Just for reference, I ran "HabitTracker % dotnet add package Microsoft.Data.Sqlite --version 10.0.9"
// into the console to install the package


class Program
{
    static void Main(string[] args)
    {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        databaseHelper.StartConnection();
        
        // Menu
        Console.WriteLine("==== Habit Tracker ====");
        Console.WriteLine("1. Create New Habit");
        Console.WriteLine("2. Update a Habit Record");
        Console.WriteLine("3. Delete a Habit Record");
        Console.WriteLine("4. View Current Habits");
        
        
    }
}
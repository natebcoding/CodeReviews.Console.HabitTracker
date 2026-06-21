namespace HabitTracker;

// Might want to update Date from a string to DateTime, leaving as string for basic testing
// Reminder to self: no constructor needed for records
public record Habit(int Id, string HabitName, int HabitQuantity, string Date);
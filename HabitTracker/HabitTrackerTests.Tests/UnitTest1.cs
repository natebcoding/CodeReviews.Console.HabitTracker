using HabitTracker;

namespace HabitTrackerTests.Tests;

public class UnitTest1
{
    
    [Fact]
    public void Check_ValidDateReturnsNonNull()
    {
        var databaseHelper = new DatabaseHelper();

        var result = databaseHelper.CheckIfValidDate("06/27/2026");

        Assert.NotNull(result);
    }

    [Fact]
    public void Check_ValidDateFormat()
    {

        var databaseHelper = new DatabaseHelper();

        var result = databaseHelper.CheckIfValidDate("6/27/26");
        
        Assert.Null(result);
    }


    [Fact]
    public void Check_OutOfBoundsDate()
    {
        var databaseHelper = new DatabaseHelper();

        var result = databaseHelper.CheckIfValidDate("6/41/2026");
        
        Assert.Null(result);
    }
}

namespace WebNet23Online.Models.HabitTracker;

public class HabitAdminStatisticViewModel
{
    public int AverageHabitsCount { get; set; }
    public float AveragePercentOfSuccess { get; set; } 
    public List<string> TrendingHabits  { get; set; }
}
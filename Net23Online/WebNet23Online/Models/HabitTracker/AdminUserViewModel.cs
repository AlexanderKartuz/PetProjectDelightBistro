using WebNet23Online.Data.Enums;

namespace WebNet23Online.Models.HabitTracker;

public class AdminUserViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserRole Role { get; set; }
    public int HabitsCount { get; set; }
    public bool IsBlocked { get; set; }
}
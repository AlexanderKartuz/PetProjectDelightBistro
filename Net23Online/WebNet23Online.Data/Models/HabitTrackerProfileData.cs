namespace WebNet23Online.Data.Models;

public class HabitTrackerProfileData : BaseModel
{
    public bool IsBlocked { get; set; }
    public int UserId { get; set; }
    
    public virtual UserData User { get; set; }
}
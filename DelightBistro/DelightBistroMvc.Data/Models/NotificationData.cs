namespace DelightBistroMvc.Data.Models;

public class NotificationData : BaseModel
{
    public string Text { get; set; }

    public DateTime TimeToPublish { get; set; }

    public bool IsActive { get; set; } = true;
    
    public virtual UserData Author { get; set; }
}

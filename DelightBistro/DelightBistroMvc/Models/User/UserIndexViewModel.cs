namespace DelightBistroMvc.Models.User;

public class UserIndexViewModel
{
    public List<UserViewModel> Users { get; set; }
    public bool IsCurrentUserAdmin { get; set; }
}

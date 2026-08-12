namespace DelightBistroMvc.Data.Models
{
    public class UserProfileData : BaseModel
    {
        public string Email { get; set; }
        public string FavBook { get; set; }
        public string PetName { get; set; }

        public virtual UserData User { get; set; }
    }
}

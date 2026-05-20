namespace WebNet23Online.Data.Models.AnimalWorld
{
    public class AnimalSpeciesData : BaseModel
    {
        public string AnimalSpeciesName { get; set; }

        public string AnimalSpeciesUrl { get; set; }

        public string NativeRange { get; set; }

        public string Description { get; set; }

        public int AnimalFamilyId { get; set; }

        public int UserId { get; set; }

        public virtual AnimalFamilyData AnimalFamily { get; set; }

        public virtual List<ZooData> ZooData { get; set; }

        public virtual UserData Creator { get; set; }
    }
}

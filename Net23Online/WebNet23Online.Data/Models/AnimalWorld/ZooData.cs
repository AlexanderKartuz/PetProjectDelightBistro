namespace WebNet23Online.Data.Models.AnimalWorld
{
    public class ZooData : BaseModel
    {
        public string ZooName { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public virtual List<AnimalSpeciesData> AnimalSpecies { get; set; }

        public virtual List<TicketData> Tickets { get; set; }

        public virtual List<CommentData> Comments { get; set; }

        public virtual UserData Creator { get; set; }
    }
}

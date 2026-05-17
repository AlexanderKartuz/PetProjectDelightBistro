namespace WebNet23Online.Data.Models
{
    public class SlayTheSpire2HeroesCards:BaseModel
    {
        public string Name { get; set;}
        public string Description { get; set;}
        public string Rarity { get; set;}
        public int ManaCost { get; set;}
        public string TypeOfCard { get; set;}
        public bool Upgraded { get; set;}
        public string ImageUrl { get; set;}
        public int HeroId { get; set;}
        public virtual SlayTheSpire2HeroesData Hero {  get; set;}
    }
}
namespace WebNet23Online.Models.LittleLemon
{
    public class LittleLemonIndexPageViewModel
    {
        public LittleLemonHeroSectionViewModel? Hero { get; set; } 
        public List<LittleLemonMenuCardsViewModel> MenuItems { get; set; }
        public List<LittleLemonTestimonialViewModel> Testimonials {  get; set; }
    }
}

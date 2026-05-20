namespace WebNet23Online.Models.LittleLemon
{
    public class LittleLemonConfirmationViewModel
    {
        public LittleLemonHeroSectionViewModel Hero { get; set; }
        public LittleLemonReservationViewModel Reservation { get; set; }
        public bool CanSeeHistory { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.CustomValidatioAttributes;
namespace WebNet23Online.Models.LittleLemon
{
    public class LittleLemonReservationViewModel
    {

        [Required(ErrorMessage = "Please enter a valid Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
        public string? GuestName { get; set; }

        [MinMaxCheck(1, 10, ErrorMessage = "Number of guests must be between 1 and 10.")]
        public int NumberOfGuests { get; set; }
        [Required(ErrorMessage = "Please select a seating preference.")]
        [AllowedStringValues("no-preference", "indoor", "patio", "window")]
        public string? SeatingPreference { get; set; }
        [Required(ErrorMessage = "Please select a reservation time.")]
        public string? AvailableTimesOnly { get; set; }
        [Required(ErrorMessage = "Please select a reservation date.")]
        public string? ReservationDateOnly { get; set; }
        [Required(ErrorMessage = "Occasion is required.")]
        [AllowedStringValues("none", "birthday", "anniversary", "other")]
        public string? Occasion { get; set; }
        [Required(ErrorMessage = "Comments are required.")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Comments Must be 3-500 characters.")]
        public string? UserComments { get; set; }
        public string? CakePhotoUrl { get; set; }
    }
}

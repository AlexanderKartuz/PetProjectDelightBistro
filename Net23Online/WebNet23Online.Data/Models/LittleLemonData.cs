using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNet23Online.Data.Models
{
    public class LittleLemonData : BaseModel
    {
        public int NumberOfGuests { get; set; }
        public string SeatingPreference { get; set; }
        public string AvailableTimesOnly { get; set; }
        public string ReservationDateOnly { get; set; }
        public string Occasion { get; set; }
        public string UserComments { get; set; }
        public int? CreatedByUserId { get; set; }
        public virtual UserData? CreatedByUser { get; set; }
        public int GuestId { get; set; }
        public virtual LittleLemonGuestData Guest { get; set; }
        public string? CakePhotoUrl { get; set; }
    }
}

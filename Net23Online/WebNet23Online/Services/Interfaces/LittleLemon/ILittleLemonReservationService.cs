using WebNet23Online.Models.LittleLemon;

namespace WebNet23Online.Services.Interfaces.LittleLemon
{
    public interface ILittleLemonReservationService
    {
        int CreateGuest(string guestName);
        int CreateReservation(LittleLemonReservationViewModel viewModel);
        LittleLemonReservationViewModel GetReservationViewModelById(int id);
        bool LinkReservationToGuest(int reservationId, int guestId);
        List<LittleLemonReservationHistoryItemViewModel> GetReservationHistoryForCurrentUser();
        void SetReservationCakePhotoUrl(int reservationId, string CakePhotoUrl);
    }
}
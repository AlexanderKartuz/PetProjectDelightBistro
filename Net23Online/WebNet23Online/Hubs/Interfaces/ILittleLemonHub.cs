namespace WebNet23Online.Hubs.Interfaces
{
    public interface ILittleLemonHub
    {
        Task NewReservationCreated(
            int reservationId,
            string guestName,
            string reservationDate,
            string time,
            int numberOfGuests,
            string seating,
            string occasion,
            string userComments,
            string? cakePhotoUrl);

        Task ReceivePrivateMessage(int senderUserId, string senderName, string message);
    }
}

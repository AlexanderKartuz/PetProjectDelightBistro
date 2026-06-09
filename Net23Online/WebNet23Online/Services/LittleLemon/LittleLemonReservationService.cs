using WebNet23Online.Services.Interfaces.LittleLemon;
using WebNet23Online.Models.LittleLemon;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Data.Models;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Data.Enums;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Hubs;
using WebNet23Online.Hubs.Interfaces;
namespace WebNet23Online.Services.LittleLemon
{
    public class LittleLemonReservationService : ILittleLemonReservationService
    {
        private ILittleLemonReservationRepository _reservationDataRepository;
        private ILittleLemonGuestRepository _guestDataRepository;
        private IAuthService _authService;
        private IHubContext<LittleLemonHub, ILittleLemonHub> _hubContext;
        private ILittleLemonChatService _chatService;

        public LittleLemonReservationService(
            ILittleLemonReservationRepository reservationDataRepository,
            ILittleLemonGuestRepository guestDataRepository,
            IAuthService authService,
            IHubContext<LittleLemonHub, ILittleLemonHub> hubContext,
            ILittleLemonChatService chatService)
        {
            _reservationDataRepository = reservationDataRepository;
            _guestDataRepository = guestDataRepository;
            _authService = authService;
            _hubContext = hubContext;
            _chatService = chatService;
        }

        public int CreateGuest(string guestName)
        {
            var existingGuest = _guestDataRepository.GetAll()
                .FirstOrDefault(x => string.Equals(x.Name, guestName));

            if (existingGuest != null)
            {
                return existingGuest.Id;
            }

            var newGuest = new LittleLemonGuestData
            {
                Name = guestName
            };

            _guestDataRepository.Add(newGuest);
            return newGuest.Id;
        }

        public int CreateReservation(LittleLemonReservationViewModel viewModel)
        {

            var guestId = CreateGuest(viewModel.GuestName!);
            var guest = _guestDataRepository.Get(guestId);
            var role = _authService.GetRole();
            var isAdmin = role == UserRole.Admin;
            var isUser = role == UserRole.User;
            var reservationData = new LittleLemonData
            {
                GuestId = guest!.Id,
                NumberOfGuests = viewModel.NumberOfGuests,
                SeatingPreference = isAdmin || isUser
                    ? viewModel.SeatingPreference
                    : "no-preference",
                AvailableTimesOnly = viewModel.AvailableTimesOnly,
                ReservationDateOnly = viewModel.ReservationDateOnly,
                Occasion = viewModel.Occasion,
                UserComments = viewModel.UserComments,
                CreatedByUserId = isAdmin || isUser ? _authService.GetUserId() : null,
            };
            _reservationDataRepository.Add(reservationData);
            return reservationData.Id;
        }

        public LittleLemonReservationViewModel? GetReservationViewModelById(int id)
        {
            var reservationDataById = _reservationDataRepository.Get(id);
            if (reservationDataById is null)
            {
                return null;
            }
            var guest = _guestDataRepository.Get(reservationDataById.GuestId);

            return new LittleLemonReservationViewModel
            {
                GuestName = guest!.Name,
                NumberOfGuests = reservationDataById.NumberOfGuests,
                SeatingPreference = reservationDataById.SeatingPreference,
                AvailableTimesOnly = reservationDataById.AvailableTimesOnly,
                ReservationDateOnly = reservationDataById.ReservationDateOnly,
                Occasion = reservationDataById.Occasion,
                UserComments = reservationDataById.UserComments,
                CakePhotoUrl = reservationDataById.CakePhotoUrl,
            };
        }

        public bool LinkReservationToGuest(int reservationId, int guestId)
        {
            var reservation = _reservationDataRepository.Get(reservationId);
            var guest = _guestDataRepository.Get(guestId);

            if (reservation == null || guest == null)
            {
                return false;
            }

            reservation.GuestId = guestId;
            _reservationDataRepository.Update(reservation);
            return true;
        }
        public List<LittleLemonReservationHistoryItemViewModel> GetReservationHistoryForCurrentUser()
        {
            var userId = _authService.GetUserId();
            var role = _authService.GetRole();
            var reservations = _reservationDataRepository.GetAll();
            if (role == UserRole.User)
            {
                reservations = reservations
                    .Where(reservationData => reservationData.CreatedByUserId == userId)
                    .ToList();
            }
            var reservationHistory = reservations
                .OrderByDescending(reservationData => reservationData.Id)
                .Select(reservationData =>
                {
                    var guest = _guestDataRepository.Get(reservationData.GuestId);
                    return new LittleLemonReservationHistoryItemViewModel
                    {
                        ReservationId = reservationData.Id,
                        Reservation = new LittleLemonReservationViewModel
                        {
                            GuestName = guest!.Name,
                            NumberOfGuests = reservationData.NumberOfGuests,
                            SeatingPreference = reservationData.SeatingPreference,
                            AvailableTimesOnly = reservationData.AvailableTimesOnly,
                            ReservationDateOnly = reservationData.ReservationDateOnly,
                            Occasion = reservationData.Occasion,
                            UserComments = reservationData.UserComments,
                            CakePhotoUrl = reservationData.CakePhotoUrl,
                        }
                    };
                })
         .ToList();
            return reservationHistory;
        }
        public void SetReservationCakePhotoUrl(int reservationId, string CakePhotoUrl)
        {
            var reservation = _reservationDataRepository.Get(reservationId);
            if (reservation == null)
            {
                return;
            }
            reservation.CakePhotoUrl = CakePhotoUrl;
            _reservationDataRepository.Update(reservation);

        }

        public bool HasReservationAtDateTime(string date, string time, string seatingPreference)
        {
            if (!_authService.IsAuthenticated())
            {
                return false;
            }

            var role = _authService.GetRole();
            if (role != UserRole.User && role != UserRole.Admin)
            {
                return false;
            }

            var userId = _authService.GetUserId();
            var userReservations = _reservationDataRepository.GetAll()
                .Where(reservation => reservation.CreatedByUserId == userId);

            if (role == UserRole.Admin)
            {
                return userReservations.Any(reservation =>
                    reservation.ReservationDateOnly == date
                    && reservation.AvailableTimesOnly == time
                    && reservation.SeatingPreference == seatingPreference);
            }

            return userReservations.Any(reservation =>
                reservation.ReservationDateOnly == date
                && reservation.AvailableTimesOnly == time);
        }

        public async Task NotifyReservationCreatedAsync(int reservationId)
        {
            var reservation = GetReservationViewModelById(reservationId)
                ?? throw new InvalidOperationException("Reservation not found.");

            await _hubContext.Clients
                .Group(_chatService.AdminGroupName)
                .NewReservationCreated(
                    reservationId,
                    reservation.GuestName ?? string.Empty,
                    reservation.ReservationDateOnly ?? string.Empty,
                    reservation.AvailableTimesOnly ?? string.Empty,
                    reservation.NumberOfGuests,
                    reservation.SeatingPreference ?? string.Empty,
                    reservation.Occasion ?? string.Empty,
                    reservation.UserComments ?? string.Empty,
                    reservation.CakePhotoUrl);
        }

    }
}


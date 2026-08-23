using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DelightBistroMvc.Controllers.CustomAuthAttribute;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Models.Notification;
using DelightBistroMvc.Services.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Models;

namespace DelightBistroMvc.Controllers
{
    [IsAdmin]
    public class NotificationController : Controller
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHub;
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationController(
            IHubContext<NotificationHub, INotificationHub> notificationHub,
            INotificationRepository notificationRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _notificationHub = notificationHub;
            _notificationRepository = notificationRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken = default)
        {
            var notifications = await _notificationRepository.GetAllAsync(cancellationToken);
            var viewModels = notifications
                .Select(x => new SingleNotificationViewModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    TimeToPublish = x.TimeToPublish,
                    AuthorName = x.Author.Name
                })
                .ToList();

            return View(viewModels);
        }

        [HttpPost]
        public IActionResult SendInstantNotification(string text)
        {
            _notificationHub.Clients.All.NewMessage(text);
            return RedirectToAction(nameof(IndexAsync));
        }


        [HttpPost]
        public async Task<IActionResult> SavePreparedNotificationAsync(
            string text,
            DateTime date,
            DateTime time,
            CancellationToken cancellationToken = default)
        {
            var timeToPublish = date;
            timeToPublish = timeToPublish.AddHours(time.Hour);
            timeToPublish = timeToPublish.AddMinutes(time.Minute);

            var user = await _authService.GetUserAsync(cancellationToken)
                ?? throw new InvalidOperationException("Current user not found");

            var dbModel = new NotificationData
            {
                Text = text,
                TimeToPublish = timeToPublish,
                Author = user
            };
            await _notificationRepository.AddAsync(dbModel, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(IndexAsync));
        }
    }
}

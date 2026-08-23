using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DelightBistroMvc.Controllers.CustomAuthAttribute;
using DelightBistroMvc.Hubs;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Models.Notification;
using DelightBistroMvc.Services.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Controllers
{
    [IsAdmin]
    public class NotificationController : Controller
    {
        private IHubContext<NotificationHub, INotificationHub> _notificationHub;
        private INotificationRepository _notificationRepository;
        private IAuthService _authService;

        public NotificationController(
            IHubContext<NotificationHub, INotificationHub> notificationHub,
            INotificationRepository notificationRepository,
            IAuthService authService)
        {
            _notificationHub = notificationHub;
            _notificationRepository = notificationRepository;
            _authService = authService;
        }


        public IActionResult Index()
        {
            var viewModels = _notificationRepository
                .GetAllAsync()
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
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult SavePreparedNotification(string text, DateTime date, DateTime time)
        {
            var timeToPublish = date;
            timeToPublish = timeToPublish.AddHours(time.Hour);
            timeToPublish = timeToPublish.AddMinutes(time.Minute);

            var user = _authService.GetUser()!;

            var dbModel = new Data.Models.NotificationData
            {
                Text = text,
                TimeToPublish = timeToPublish,
                Author = user
            };
            _notificationRepository.AddAsync(dbModel);
            return RedirectToAction(nameof(Index));
        }
    }
}

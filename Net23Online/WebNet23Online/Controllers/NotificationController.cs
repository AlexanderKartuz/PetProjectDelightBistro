using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Hubs;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Models.Notification;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
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
                .GetAll()
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
            _notificationRepository.Add(dbModel);
            return RedirectToAction(nameof(Index));
        }
    }
}

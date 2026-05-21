using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Enums;
using WebNet23Online.Models.Tickets;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ITicketService _ticketService;
        private IAuthService _authService;

        public TicketsController(ITicketService ticketService, IAuthService authService)
        {
            _ticketService = ticketService;
            _authService = authService;
        }

        public IActionResult AllMyTickets()
        {
            var userId = _authService.GetUserId();
            var zooTickets = _ticketService.GetUserZooTickets(userId);
            var viewModel = new AllTicketsViewModel
            {
                CanShowZooTickets = zooTickets.Any(),
                ZooTickets = zooTickets,
            };
            return View(viewModel);
        }

        [CanReserveZooVisit]
        [HttpPost]
        public IActionResult ZooReservations(string zooName)
        {
            _ticketService.BookZoo(zooName, TicketType.ZooVisit);
            return View();
        }

        public IActionResult ZooReservationsDenied()
        {
            return View();
        }
    }
}

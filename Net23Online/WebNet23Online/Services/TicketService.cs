using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;
using WebNet23Online.Models.Tickets;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class TicketService : ITicketService
    {
        private IAuthService _authService;
        private ITicketRepository _ticketRepository;
        private IZooRepository _zooRepository;

        public TicketService(IAuthService authService, ITicketRepository ticketRepository, IZooRepository zooRepository)
        {
            _authService = authService;
            _ticketRepository = ticketRepository;
            _zooRepository = zooRepository;
        }

        public void BookZoo(string zooName, TicketType type)
        {
            var user = _authService.GetUser();
            var zoo = _zooRepository.GetElementByName(zooName);
            TicketData ticketData = new TicketData
            {
                User = user,
                Zoo = zoo,
                TicketType = type,
                EventDate = DateTime.UtcNow.AddMonths(1),
                UniqueKey = Guid.NewGuid().ToString()
            };

            _ticketRepository.Add(ticketData);
        }

        public List<ZooTicketsViewModel> GetUserZooTickets(int userId)
        {
            var zooTicketsData = _ticketRepository.GetUserZooTickets(userId);
            var zooTicketsViewModel = zooTicketsData.Select(x => new ZooTicketsViewModel
            {
                UniqueKey = x.UniqueKey,
                ZooName = x.Zoo.ZooName,
                IsUsed = x.IsUsed,
                EventDate = x.EventDate,
            }).ToList();
            return zooTicketsViewModel;
        }
    }
}

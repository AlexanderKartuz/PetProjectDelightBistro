using WebNet23Online.Data.Enums;
using WebNet23Online.Models.Tickets;

namespace WebNet23Online.Services.Interfaces
{
    public interface ITicketService
    {
        void BookZoo(string zooName, EntityType type);
        List<ZooTicketsViewModel> GetUserZooTickets(int userId);
    }
}

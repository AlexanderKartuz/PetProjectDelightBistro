using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface ITicketRepository : IBaseRepository<TicketData>
    {
        List<TicketData> GetUserZooTickets(int userId);
    }
}

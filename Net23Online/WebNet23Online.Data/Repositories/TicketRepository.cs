using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories
{
    public class TicketRepository : BaseRepository<TicketData>, ITicketRepository
    {
        public TicketRepository(WebContext context) : base(context)
        {
        }

        public List<TicketData> GetUserZooTickets(int userId)
        {
            return _dbSet.Where(x => x.UserId == userId && x.TicketType == EntityType.Zoo).Include(x => x.Zoo).ToList();
        }
    }
}

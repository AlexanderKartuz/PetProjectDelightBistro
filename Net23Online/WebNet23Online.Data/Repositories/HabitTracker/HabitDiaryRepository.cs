using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories;

public class HabitDiaryRepository : BaseRepository<HabitTrackerDiaryData>, IHabitDiaryRepository
{
    private IHabitDiaryRepository _habitDiaryRepository;
    
    public HabitDiaryRepository(WebContext webContext) : base(webContext)
    {
    }

    public List<HabitTrackerDiaryData> GetByUserAndMonth(UserData user, int year, int month)
    {
        return _dbSet
            .Where(x=> x.User.Id == user.Id 
                       && x.Date.Year == year
                       && x.Date.Month == month)
            .ToList();
    }
    
    public HabitTrackerDiaryData? GetByUserAndDate (UserData user, DateTime date)
    {
        return _dbSet
            .FirstOrDefault(x=> x.User.Id == user.Id 
                                && x.Date.Date == date.Date);
    } 
}
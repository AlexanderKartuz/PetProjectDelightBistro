using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories;

public class HabitDoneDatesRepository : BaseRepository<HabitDoneDatesData>, IHabitDoneDatesRepository
{
    public HabitDoneDatesRepository(WebContext context) : base(context) { }
    public void ChangeDayPointStatus(int habitId, int dayOfWeek)
    {
        var today = DateTime.Today;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var weekStart = today.AddDays(-1* diff);
        var targetDate = weekStart.AddDays(dayOfWeek);

        var targetDateDone = _dbSet
            .Where(x => x.HabitId == habitId)
            .FirstOrDefault(x => x.DateOfCompletion.Date == targetDate.Date);
    
        if (targetDateDone == null)
        {
            var habit = _context.Habits.FirstOrDefault(x => x.Id == habitId);

            if (habit == null)
            {
                throw new ArgumentNullException("Привычка не найдена");
            }
            
            Add(new HabitDoneDatesData
            {
                DateOfCompletion = targetDate.Date,
                Habit = habit
            });
        }
        else
        {
            Remove(targetDateDone);
        }
    }
}
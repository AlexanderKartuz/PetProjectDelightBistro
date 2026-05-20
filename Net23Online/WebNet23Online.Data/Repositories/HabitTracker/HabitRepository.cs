using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces;

public class HabitRepository : BaseRepository<HabitData>, IHabitRepository
{
    public HabitRepository(WebContext webContext) : base(webContext)
    {
    }

    public bool UserHasHabits(int userId)
    {
        return _context.Habits.Any(h => h.UserId == userId);
    }

    public List<HabitData> GetByUserId(int userId)
    {
        return _dbSet
            .Where(x => x.User.Id == userId)
            .ToList();
    }

    public List<HabitData> GetByUserIdWithDatesForCurrentWeek(int userId)
    {
        var today = DateTime.Today;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var weekStart = today.AddDays(-1 * diff);

        return _dbSet
            .Include(x => x.CompletedDates
                .Where(d => d.DateOfCompletion >= weekStart && d.DateOfCompletion < weekStart.AddDays(7)))
            .Where(x => x.User.Id == userId)
            .ToList();
    }

    public List<HabitData> GetByUserIdWithDatesForCurrentMonth(int userId)
    {
        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        return _dbSet
            .Include(x => x.CompletedDates
                .Where(d => d.DateOfCompletion >= monthStart && d.DateOfCompletion < monthEnd))
            .Where(x => x.User.Id == userId)
            .ToList();
    }

    public List<HabitData> AddDefaultHabits(int userId)
    {
        var habitData = new List<HabitData>();
        var defaultHabits = new List<string> { "Спорт 30 мин", "Вода 2л" };

        foreach (var title in defaultHabits)
        {
            var habit = new HabitData()
            {
                Title = title,
                MonthGoal = 30, //default
                UserId = userId
            };
            habitData.Add(habit);
            _dbSet.Add(habit);
            _context.SaveChanges();
        }

        return habitData;
    }

    public void EditHabit(HabitData habitData)
    {
        var habit = _dbSet.FirstOrDefault(x => x.Id == habitData.Id);
        if (habit == null)
        {
            return;
        }

        habit.Title = habitData.Title;
        habit.MonthGoal = habitData.MonthGoal;
        Update(habit);
    }

    public List<string> GetTitlesByUserId(int userId)
    {
        return _dbSet
            .Where(x => x.UserId == userId)
            .Select(x => x.Title)
            .ToList();
    }

    public bool IsHabitTitleUniq(string title, int userId, int habitId)
    {
        return !_dbSet.Any(x => x.UserId == userId && x.Title == title && x.Id != habitId);
    }

    public int GetHabitsCount(int userId)
    {
        return _dbSet
            .Count(x => x.UserId == userId);
    }
}
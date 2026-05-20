using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces;

public interface IHabitRepository : IBaseRepository<HabitData>
{
    bool UserHasHabits(int userId);
    public List<HabitData> GetByUserId(int userId);
    List<HabitData> GetByUserIdWithDatesForCurrentWeek(int userId);
    List<HabitData> GetByUserIdWithDatesForCurrentMonth(int userId);
    List<HabitData> AddDefaultHabits(int userId);
    void EditHabit(HabitData habitData);
    List<string> GetTitlesByUserId(int userId);
    bool IsHabitTitleUniq(string title, int userId, int habitId);
    int GetHabitsCount(int userId);
}
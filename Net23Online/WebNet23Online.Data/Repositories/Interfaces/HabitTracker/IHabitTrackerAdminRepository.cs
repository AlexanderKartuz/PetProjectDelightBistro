using WebNet23Online.Data.Models;

namespace WebNet23Online.Data.Repositories.Interfaces.HabitTracker;

public interface IHabitTrackerAdminRepository : IBaseRepository<HabitTrackerProfileData>
{
    HabitTrackerProfileData? GetByUserId(int userId);
    void BlockUser(int userId);
    void UnblockUser(int userId);
    float GetAveragePercentOfSuccess();
    int GetAverageHabitsCount();
    List<string> GetTrendingHabits();


}
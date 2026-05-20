using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories;

public interface IHabitDiaryRepository: IBaseRepository<HabitTrackerDiaryData>
{
    List<HabitTrackerDiaryData> GetByUserAndMonth(UserData user, int year, int month);
    HabitTrackerDiaryData? GetByUserAndDate(UserData user, DateTime date);
}
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Data.Repositories;

public interface IHabitDoneDatesRepository : IBaseRepository<HabitDoneDatesData>
{
    void ChangeDayPointStatus(int habitId, int dayOfWeek);
}
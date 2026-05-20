using System.Data;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories;
using WebNet23Online.Models.HabitTracker;

namespace WebNet23Online.Services;

public class HabitStatisticsService : IHabitStatisticsService
{
    public HabitTrackerViewModel CreateStatisticsInfo(List<HabitData> habitData)
    {
        var today = DateTime.Today;
        var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

        return new HabitTrackerViewModel
        {
            Habits = habitData.Select(habit =>
            {
                var doneCount = habit.CompletedDates?.Count ?? 0;

                return new HabitViewModel
                {
                    Id = habit.Id,
                    Title = habit.Title,
                    DaysInMonth = daysInMonth,
                    DoneCountInMonth = doneCount,
                    Percent = habit.MonthGoal > 0 
                        ? (float)doneCount / habit.MonthGoal * 100 
                        : 0,
                    UserId = habit.UserId,
                    MonthGoal = habit.MonthGoal,
                };
            }).ToList()
        };
    }
}
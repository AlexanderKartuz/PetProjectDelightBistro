using Microsoft.EntityFrameworkCore;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.HabitTracker;

namespace WebNet23Online.Data.Repositories;

public class HabitTrackerAdminRepository : BaseRepository<HabitTrackerProfileData>, IHabitTrackerAdminRepository
{
    public HabitTrackerAdminRepository(WebContext context) : base(context) { }

    public HabitTrackerProfileData? GetByUserId(int userId)
    {
        return _dbSet.FirstOrDefault(x => x.UserId == userId);
    }

    public void BlockUser(int userId)
    {
        var profile = GetByUserId(userId);
        profile.IsBlocked = true;
        Update(profile);
    }

    public void UnblockUser(int userId)
    {
        var profile = GetByUserId(userId);
        profile.IsBlocked = false;
        Update(profile);
    }

    public float GetAveragePercentOfSuccess()
    {
        var sql = @"
            Select Round(AVG(AvgPercent), 2) as Value
            From(
                SELECT UserId, AVG((DoneDates * 100.0) / MonthGoal) AvgPercent 
                From( 
                    Select UserId, MonthGoal, COUNT(*) AS DoneDates  
                    From Habits h
                    Join HabitDoneDates hdd on hdd.HabitId = h.Id 
                    Group By h.Id, h.UserId, h.MonthGoal  
                ) sub  
                Group By UserId
            ) sub2";


        
        // var result = _context.Habits
        //     .Select(h => new
        //     {
        //         h.UserId,
        //         Percent = h.MonthGoal > 0
        //             ? ((float)h.CompletedDates.Count / h.MonthGoal) * 100
        //             : 0
        //     })
        //     .AsEnumerable()
        //     .GroupBy(x => x.UserId)
        //     .Select(g => g.Average(x => x.Percent))
        //     .Average();

        var result = _context
            .Database
            .SqlQueryRaw<decimal>(sql)
            .FirstOrDefault();
        
        return (float)result;
    }
    
    public int GetAverageHabitsCount()
    {
        var result = _context.Habits
            .GroupBy(h => h.UserId)
            .Select(g => g.Count())
            .Average();

        return (int)result;
    }
    
    public List<string> GetTrendingHabits()
    {
        var sql = @"
            SELECT TOP 5 FirstWord
            FROM (
                SELECT 
                    CASE
                        WHEN CHARINDEX(' ', Title) > 0
                        THEN LOWER(SUBSTRING(Title, 1, CHARINDEX(' ', Title)-1))
                        ELSE LOWER(Title)
                    END AS FirstWord
                FROM Habits
            ) allHabits
            GROUP BY FirstWord
            ORDER BY COUNT(*) DESC";
        
        
        // var trendingHabits = _context.Habits
        //     .Select(h => h.Title)
        //     .AsEnumerable()
        //     .GroupBy(h => h
        //         .Trim()
        //         .Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]
        //         .ToLower())
        //     .OrderByDescending(g => g.Count())
        //     .Take(5)
        //     .Select(g => g.Key)
        //     .ToList();
        
        return _context
            .Database
            .SqlQueryRaw<string>(sql)
            .ToList();
    }
}
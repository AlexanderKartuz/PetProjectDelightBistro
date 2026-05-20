using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Data.Repositories.Interfaces.HabitTracker;
using WebNet23Online.Models.HabitTracker;

namespace WebNet23Online.Controllers;

[IsModerator]
public class HabitTrackerAdminController : Controller
{
    private IHabitRepository _habitRepository;
    private IUserRepository _userRepository;
    private IHabitTrackerAdminRepository _habitTrackerAdminRepository;

    public HabitTrackerAdminController(IHabitRepository habitRepository,
        IHabitTrackerAdminRepository habitTrackerAdminRepository, IUserRepository userRepository)
    {
        _habitRepository = habitRepository;
        _habitTrackerAdminRepository = habitTrackerAdminRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult AdminPanel()
    {
        var model = new HabitAdminStatisticViewModel
        {
            AverageHabitsCount = _habitTrackerAdminRepository.GetAverageHabitsCount(),
            AveragePercentOfSuccess = _habitTrackerAdminRepository.GetAveragePercentOfSuccess(),
            TrendingHabits = _habitTrackerAdminRepository.GetTrendingHabits()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult UserList()
    {
        var users = _userRepository.GetAll();
        var profiles = _habitTrackerAdminRepository.GetAll();

        var model = users.Select(u => new AdminUserViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Role = u.Role,
            HabitsCount = _habitRepository.GetHabitsCount(u.Id),
            IsBlocked = profiles
                .FirstOrDefault(p => p.UserId == u.Id)?
                .IsBlocked ?? false
        }).ToList();

        return View(model);
    }

    [HttpPost]
    public IActionResult ToggleBlock(int userId)
    {
        var profile = _habitTrackerAdminRepository.GetByUserId(userId);
        if (profile == null || !profile.IsBlocked)
            _habitTrackerAdminRepository.BlockUser(userId);
        else
            _habitTrackerAdminRepository.UnblockUser(userId);

        return RedirectToAction(nameof(UserList));
    }
}
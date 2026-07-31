using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Models;
using WebNet23Online.Models.Home;
using WebNet23Online.Services;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger,
            IUserRepository userRepository,
            IAuthService authService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _authService = authService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            var user = _authService.GetUser();
            if (user is not null)
            {
                viewModel.UserName = user.Name;
                viewModel.RoleName = user.Role.ToString();
            }
            else
            {
                viewModel.UserName = "Guest";
            }

            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces.DelightBistro;
using WebNet23Online.Models.Auth;
using WebNet23Online.Services;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class AuthController : Controller
    {
        private IUserRepository _userRepository;
        private IAuthService _authService;

        public AuthController(IUserRepository userRepository,
            IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = _userRepository.GetByNameAndPassword(viewModel.Login, viewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError(
                    nameof(LoginViewModel.Login), //"Login"
                    "There is no User with this login and password");
                return View(viewModel);
            }

            await _authService.SignIn(user);

            return RedirectToAction("Index", "DelightBistro");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(LoginViewModel viewModel)
        {
            if (!_userRepository.IsNameUniq(viewModel.Login))
            {
                ModelState.AddModelError(nameof(LoginViewModel.Login), "Name is already used");
                return View(viewModel);
            }

            var user = new UserData
            {
                Name = viewModel.Login,
                PasswordHash = viewModel.Password,
            };

            _userRepository.Registration(user);

            return RedirectToAction("Index", "DelightBistro");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "DelightBistro");
        }

        public IActionResult Deny()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Models.Auth;
using DelightBistroMvc.Services.Interfaces;
using DelightBistroMvc.Data.Services.UserService;

namespace DelightBistroMvc.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private readonly IUserDataService _userDataService;

        public AuthController(
            IAuthService authService,
            IUserDataService userDataService)
        {
            _authService = authService;
            _userDataService = userDataService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(
            LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userDataService
                .ValidateCredetialsAsync(viewModel.Login, viewModel.Password);
            if (user == null)
            {
                ModelState.AddModelError(
                    nameof(LoginViewModel.Login), //"Login"
                    "There is no User with this login and password");
                return View(viewModel);
            }

            await _authService.SignInAsync(user);

            return RedirectToAction("Index", "DelightBistro");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAsync(
            RegisterViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            if (!_userDataService.IsNameUniq(viewModel.Login))
            {
                ModelState.AddModelError(nameof(LoginViewModel.Login),
                    "Name is already used");
                return View(viewModel);
            }

            var user = new UserData
            {
                Name = viewModel.Login,
                PasswordHash = viewModel.Password,

                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Mobilephone = viewModel.Mobilephone,
            };

            await _userDataService.RegisterAsync(user, cancellationToken);
            await _authService.SignInAsync(user);

            return RedirectToAction("Index", "DelightBistro");
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "DelightBistro");
        }

        public IActionResult Deny()
        {
            return View();
        }
    }
}

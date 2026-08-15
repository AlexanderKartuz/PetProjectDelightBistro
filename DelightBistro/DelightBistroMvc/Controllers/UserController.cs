using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using DelightBistroMvc.Controllers.CustomAuthAttribute;
using DelightBistroMvc.Data.Enums;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.User;
using DelightBistroMvc.Services;
using DelightBistroMvc.Services.Interfaces;
using DelightBistroMvc.Data.Services.UserService;

namespace DelightBistroMvc.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IAuthService _authService;
        public IUserRepository _userRepository;
        public IWebHostEnvironment _webHostEnvironment;
        private IUserDataService _userDataService;

        public UserController(IAuthService authService,
            IUserRepository userRepository,
            IWebHostEnvironment webHostEnvironment,
            IUserDataService userDataService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _userDataService = userDataService;
        }

        [IsModerator]
        public IActionResult Index(int cardId)
        {
            var usersFromDb = _userRepository.GetAll();
            var currentUser = _authService.GetUser()!;
            var viewModel = new UserIndexViewModel
            {
                Users = usersFromDb
                    .Select(x => new UserViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),
                IsCurrentUserAdmin = currentUser.Role == UserRole.Admin,
            };

            return View(viewModel);
        }

        public IActionResult Profile()
        {
            var currentUserLanguage = _authService.GetLanguage();
            var allLanguagesList = Enum
                .GetNames<Language>()
                .Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x,
                    Selected = x == currentUserLanguage.ToString()
                })
                .ToList();

            var viewModel = new UserProfileViewModel
            {
                UserId = _authService.GetUserId(),
                UserName = _authService.GetUserName() ?? "unnamed",
                Language = currentUserLanguage,
                Languages = allLanguagesList,
                AvatarUrl = _authService.GetUser().AvatarUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLanguageAsync(int userId, Language language)
        {
            _userDataService.UpdateLanguage(userId, language);
            var user = _authService.GetUser();

            await HttpContext.SignOutAsync();

            await _authService.SignIn(user);

            return RedirectToAction(nameof(Profile));
        }

        [Authorize]
        public IActionResult UpdateAvatar(IFormFile avatar)
        {
            var user = _authService.GetUser()!;
            var userId = user.Id;
            var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
            var pathToFolder = "images\\avatars";
            var fileName = $"avatar-{userId}.jpg";

            var path = Path.Combine(pathToWwwRootFolder, pathToFolder, fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                avatar.CopyTo(fileStream); // copy to our PC
            }

            user.AvatarUrl = $"/images/avatars/{fileName}";
            _userRepository.Update(user);

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileAsync(UserProfileViewModel viewModel)
        {
            var user = _authService.GetUser();
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Mobilephone = viewModel.Mobilephone;
            _userDataService.UpdateProfile(user);
            await HttpContext.SignOutAsync();
            await _authService.SignIn(user);
            return RedirectToAction(nameof(Profile));
        }

        [IsAdmin]
        public IActionResult DeleteUser()
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GenerateReport()
        {
            var path = System.IO.Path.GetTempFileName();
            using (var file = System.IO.File.CreateText(path))
            {
                file.WriteLine($"Id,Name,Language");
                var users = _userRepository.GetAll();
                foreach (var user in users)
                {
                    file.WriteLine($"{user.Id},{user.Name},{user.Language}");
                }
            }

            var fileStrem = new FileStream(path, FileMode.Open);
            return File(fileStrem, "text/csv");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAccountAsync(int userId)
        {
            var currentUserId = _authService.GetUserId();
            if (currentUserId != userId)
            {
                return Forbid();
            }

            var user = _authService.GetUser();
            if (!string.IsNullOrEmpty(user?.AvatarUrl))
            {
                var avatarPath = Path.Combine(_webHostEnvironment.WebRootPath,
                                              user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }
            }

            _userRepository.Delete(userId);
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}

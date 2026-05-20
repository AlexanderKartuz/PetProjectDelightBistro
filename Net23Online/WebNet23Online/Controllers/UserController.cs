using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.Steam;
using WebNet23Online.Models.User;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IAuthService _authService;
        public IUserRepository _userRepository;
        public IWebHostEnvironment _webHostEnvironment;

        public UserController(IAuthService authService,
            IUserRepository userRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _authService = authService;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult ChangeLanguage(int userId, Language language)
        {
            _userRepository.UpdateLanguage(userId, language);
            var user = _authService.GetUser();

            HttpContext.SignOutAsync().Wait();

            _authService.SignIn(user);

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
        public IActionResult UpdateProfile(UserProfileViewModel viewModel)
        {
            var user = _authService.GetUser();
            user.Id = viewModel.UserId;
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Mobilephone = viewModel.Mobilephone;
            user.Language = viewModel.Language;
            _userRepository.UpdateProfile(user);
            //var user = _authService.GetUser();

            HttpContext.SignOutAsync().Wait();

            _authService.SignIn(user);

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

        [HttpGet]
        public IActionResult SteamProfile()
        {
            var user = _authService.GetUser();
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

            var viewModel = new SteamUserProfileViewModel
            {
                UserId = _authService.GetUserId(),
                UserName = _authService.GetUserName() ?? "unnamed",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobilephone = user.Mobilephone,
                Language = currentUserLanguage,
                Languages = allLanguagesList,
                AvatarUrl = user.AvatarUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateSteamProfile(SteamUserProfileViewModel viewModel)
        {
            var user = _authService.GetUser();
            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Mobilephone = viewModel.Mobilephone;

            _userRepository.UpdateProfile(user);

            HttpContext.SignOutAsync().Wait();
            _authService.SignIn(user);

            return RedirectToAction(nameof(SteamProfile));
        }

        [HttpPost]
        public IActionResult ChangeLanguageInSteam(int userId, Language language)
        {
            _userRepository.UpdateLanguage(userId, language);
            var user = _authService.GetUser();

            HttpContext.SignOutAsync().Wait();
            _authService.SignIn(user);

            return RedirectToAction(nameof(SteamProfile));
        }

        [HttpPost]
        public IActionResult UpdateSteamAvatar(IFormFile avatar)
        {
            if (avatar == null || avatar.Length == 0)
            {
                return RedirectToAction(nameof(SteamProfile));
            }

            var user = _authService.GetUser()!;
            var userId = user.Id;
            var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
            var pathToFolder = Path.Combine("images", "steam", "avatars");
            var fullPath = Path.Combine(pathToWwwRootFolder, pathToFolder);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var fileName = $"avatar-{userId}.jpg";
            var path = Path.Combine(fullPath, fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                avatar.CopyTo(fileStream);
            }

            user.AvatarUrl = $"/{pathToFolder.Replace("\\", "/")}/{fileName}";
            _userRepository.Update(user);

            return RedirectToAction(nameof(SteamProfile));
        }

        [HttpPost]
        public IActionResult DeleteAccount(int userId)
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
            HttpContext.SignOutAsync().Wait();

            return RedirectToAction(nameof(SteamController.Index), "Steam");
        }
    }
}

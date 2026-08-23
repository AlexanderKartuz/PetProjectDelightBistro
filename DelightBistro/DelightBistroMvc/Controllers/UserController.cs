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
using DelightBistroMvc.Data.Repositories.Interfaces;

namespace DelightBistroMvc.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public readonly IAuthService _authService;
        public readonly IUserRepository _userRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserDataService _userDataService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IAuthService authService,
            IUserRepository userRepository,
            IWebHostEnvironment webHostEnvironment,
            IUserDataService userDataService,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _userDataService = userDataService;
            _unitOfWork = unitOfWork;
        }

        [IsModerator]
        public async Task<IActionResult> IndexAsync(int cardId,
            CancellationToken cancellationToken = default)
        {
            var usersFromDb = await _userRepository.GetAllAsync(cancellationToken);
            var currentUser = await _authService.GetUserAsync(cancellationToken)
                ?? throw new InvalidOperationException("Current user not found");
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

        [Authorize]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken = default)
        {
            var cuurentUser = await _authService.GetUserAsync(cancellationToken)
                ?? throw new InvalidOperationException("Current user not found");

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
                AvatarUrl = cuurentUser.AvatarUrl,
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeLanguageAsync(
            int userId,
            Language language, CancellationToken cancellationToken = default)
        {
            await _userDataService.UpdateLanguageAsync(userId, language, cancellationToken: cancellationToken);
            var user = await _authService.GetUserAsync()
                ?? throw new InvalidOperationException("Current user not found");

            await HttpContext.SignOutAsync();
            await _authService.SignInAsync(user);

            return RedirectToAction(nameof(Profile));
        }

        [Authorize]
        public async Task<IActionResult> UpdateAvatarAsync(
            IFormFile avatar,
            CancellationToken cancellationToken = default)
        {
            var user = await _authService.GetUserAsync(cancellationToken)!;
            var userId = user.Id;
            var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
            var pathToFolder = "images\\avatars";
            var fileName = $"avatar-{userId}.jpg";

            var path = Path.Combine(pathToWwwRootFolder, pathToFolder, fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await avatar.CopyToAsync(fileStream); // copy to our PC
            }

            user.AvatarUrl = $"/images/avatars/{fileName}";
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileAsync(
            UserProfileViewModel viewModel,
            CancellationToken cancellationToken = default)
        {
            var user = await _authService.GetUserAsync(cancellationToken);

            user.FirstName = viewModel.FirstName;
            user.LastName = viewModel.LastName;
            user.Mobilephone = viewModel.Mobilephone;

            await _userDataService.UpdateProfileAsync(user, cancellationToken);
            await HttpContext.SignOutAsync();
            await _authService.SignInAsync(user);

            return RedirectToAction(nameof(Profile));
        }

        [IsAdmin]
        public IActionResult DeleteUser()
        {
            return RedirectToAction(nameof(IndexAsync));
        }

        public async Task<IActionResult> GenerateReportAsync(CancellationToken cancellationToken = default)
        {
            var path = System.IO.Path.GetTempFileName();
            using (var file = System.IO.File.CreateText(path))
            {
                file.WriteLine($"Id,Name,Language");
                var users = await _userRepository.GetAllAsync(cancellationToken);
                foreach (var user in users)
                {
                    await file.WriteLineAsync($"{user.Id},{user.Name},{user.Language}");
                }
            }

            var fileStrem = new FileStream(path, FileMode.Open);
            return File(fileStrem, "text/csv");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAccountAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var currentUserId = _authService.GetUserId();
            if (currentUserId != userId)
            {
                return Forbid();
            }

            var user = await _authService.GetUserAsync(cancellationToken)
                ?? throw new InvalidOperationException("Current user not found");
            if (!string.IsNullOrEmpty(user?.AvatarUrl))
            {
                var avatarPath = Path.Combine(_webHostEnvironment.WebRootPath,
                                              user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(avatarPath))
                {
                    System.IO.File.Delete(avatarPath);
                }
            }

            await _userRepository.DeleteAsync(userId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(HomeController.IndexAsync), "Home");
        }
    }
}

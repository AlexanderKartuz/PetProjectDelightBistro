using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Controllers.CustomAuthAttribute.Steam;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Hubs;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Models.Steam;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.Steam;

namespace WebNet23Online.Controllers
{
    [Authorize]
    public class SteamController : Controller
    {
        private const int CatalogDefaultPageSize = 12;
        private const int CatalogMaxPageSize = 48;

        private readonly ICatalogService _catalogService;
        private readonly IAuthService _authService;
        private readonly IGameReviewRepository _gameReviewRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHubContext<SteamChatHub, ISteamChatHub> _steamChatHub;
        private readonly IHubContext<SteamNotificationHub, ISteamNotificationHub> _steamNotificationHub;

        public SteamController(
            ICatalogService catalogService,
            IAuthService authService,
            IGameReviewRepository gameReviewRepository,
            IWebHostEnvironment webHostEnvironment,
            IHubContext<SteamChatHub, ISteamChatHub> steamChatHub,
            IHubContext<SteamNotificationHub, ISteamNotificationHub> steamNotificationHub)

        {
            _catalogService = catalogService;
            _authService = authService;
            _gameReviewRepository = gameReviewRepository;
            _webHostEnvironment = webHostEnvironment;
            _steamChatHub = steamChatHub;
            _steamNotificationHub = steamNotificationHub;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var viewModel = _catalogService.GetGamesForHomePage();

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Catalog([FromQuery] CatalogFilterViewModel filter)
        {
            filter ??= new CatalogFilterViewModel();
            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (filter.PageSize < 1)
            {
                filter.PageSize = CatalogDefaultPageSize;
            }
            else if (filter.PageSize > CatalogMaxPageSize)
            {
                filter.PageSize = CatalogMaxPageSize;
            }

            var model = _catalogService.GetCatalog(filter);
            model.IsUserAtLeastModerator = _authService.AtLeastModerator();

            return View(model);
        }

        [HttpGet]
        [IsModerator]
        public IActionResult AddGame()
        {
            var viewModel = new AddGameViewModel
            {
                AllGenres = _catalogService.GetListItemsWithGameGenres(),
                Publishers = _catalogService.GetListItemsWithPublishers()
            };

            return View(viewModel);
        }

        [HttpPost]
        [IsModerator]
        public IActionResult AddGame(AddGameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AllGenres = _catalogService.GetListItemsWithGameGenres();
                viewModel.Publishers = _catalogService.GetListItemsWithPublishers();
                return View(viewModel);
            }
            _catalogService.AddGame(viewModel);
            _steamNotificationHub.Clients.All.NewGameAdded(viewModel.Title, viewModel.ImageUrl);

            return RedirectToAction(nameof(Catalog));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GameDetails(int id)
        {
            var gameData = _catalogService.GetGameDetails(id);

            if (gameData == null)
            {
                return NotFound();
            }

            var viewModel = new GameDetailsViewModel
            {
                Id = gameData.Id,
                Title = gameData.Title,
                Description = gameData.Description,
                ImageUrl = gameData.ImageUrl,
                Price = gameData.Price,
                Genres = gameData.GameGenres
                    .Select(g => g.Name)
                    .ToList(),
                PublisherName = gameData.Publisher?.Name ?? "Unknown",
                PublisherId = gameData.PublisherId,
                IsUserAtLeastModerator = _authService.AtLeastModerator(),
                HasUserReviewed = _authService.IsAuthenticated()
                    && _gameReviewRepository.ExistsForUser(id, _authService.GetUserId()),
                Reviews = gameData.GameReviews
                    .Select(r => new GameReviewViewModel()
                    {
                        Id = r.Id,
                        GameId = r.GameId,
                        Text = r.Text,
                        Rating = r.Rating,
                        IsRecommended = r.Rating >= 7,
                        AuthorName = r.Author.Name,
                        CreatedAt = r.CreatedAt,
                        ModifiedAt = r.ModifiedAt,
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        [EditForCreatorWithRequiredRole]
        public IActionResult EditGame(int id)
        {
            var game = _catalogService.GetGameDetails(id);

            if (game == null)
            {
                return NotFound();
            }

            var viewModel = new EditGameViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                ImageUrl = game.ImageUrl,
                Price = game.Price,
                PublisherId = game.PublisherId,
                AllGenres = _catalogService.GetListItemsWithGameGenres(),
                Publishers = _catalogService.GetListItemsWithPublishers()
            };

            return View(viewModel);
        }

        [HttpPost]
        [EditForCreatorWithRequiredRole]
        public IActionResult EditGame(EditGameViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AllGenres = _catalogService.GetListItemsWithGameGenres();
                viewModel.Publishers = _catalogService.GetListItemsWithPublishers();
                return View(viewModel);
            }

            _catalogService.UpdateGame(viewModel);

            return RedirectToAction(nameof(GameDetails), new { id = viewModel.Id });
        }

        [HttpGet]
        [DeleteWithRoleAndTimeRestriction(AllowedDaysForCreator = 7, RequiredRoleForCreator = UserRole.Moderator)]
        public IActionResult DeleteGame(int id)
        {
            _catalogService.DeleteGame(id);
            return RedirectToAction(nameof(Catalog));
        }

        [HttpGet]
        public IActionResult CommunityChat()
        {
            return View();
        }
    }
}

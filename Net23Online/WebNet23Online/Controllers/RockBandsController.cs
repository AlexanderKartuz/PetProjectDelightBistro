using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Enums;
using WebNet23Online.Models.RockBands;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace WebNet23Online.Controllers
{
    public class RockBandsController : Controller
    {
        private readonly IRockBandsService _rockBandsService;
        private readonly IAuthService _authService;
        private readonly IHubContext<RockBandHub, IRockBandHub> _rockBandHub;

        public RockBandsController(IRockBandsService rockBandsService, IAuthService authService, IHubContext<RockBandHub, IRockBandHub> rockBandHub)
        {
            _rockBandsService = rockBandsService;
            _authService = authService;
            _rockBandHub = rockBandHub;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] int[]? genreIds, [FromQuery] int? editBandId)
        {
            var selectedGenreIds = genreIds ?? Array.Empty<int>();
            var genres = _rockBandsService.GetGenres();
            foreach (var g in genres)
            {
                g.IsSelected = selectedGenreIds.Contains(g.Id);
            }

            var isAuth = _authService.IsAuthenticated();
            var currentUserId = isAuth ? _authService.GetUserId() : (int?)null;
            var viewModel = new RockBandsIndexViewModel
            {
                IsUserAuth = isAuth,
                CanEditRockBandGenres = isAuth && _authService.GetRole() == UserRole.RockBandOwner,
                Bands = _rockBandsService.GetBands(selectedGenreIds, currentUserId),
                Genres = genres,
                SelectedGenreIds = selectedGenreIds,
                EditBandId = editBandId,
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(RockBandsIndexViewModel viewModel, IFormFile? Image)
        {
            var band = viewModel.BandBlock;
            if (Image != null)
            {
                band.PhotoOfTheBand = Image;
            }
            if (!ModelState.IsValid)
            {
                var genres = _rockBandsService.GetGenres();
                var isAuth = _authService.IsAuthenticated();
                var currentUserId = isAuth ? _authService.GetUserId() : (int?)null;
                var startViewModel = new RockBandsIndexViewModel
                {
                    IsUserAuth = isAuth,
                    CanEditRockBandGenres = isAuth && _authService.GetRole() == UserRole.RockBandOwner,
                    Bands = _rockBandsService.GetBands(Array.Empty<int>(), currentUserId),
                    Genres = genres,
                    SelectedGenreIds = Array.Empty<int>(),
                    EditBandId = null,
                    BandBlock = band,
                };
                return View(startViewModel);
            }

            var createdByUserId = _authService.GetUserId();
            _rockBandsService.AddBand(band, createdByUserId);

            _rockBandHub.Clients.All.NewRockBandWasCreated(
                band.Name,
                band.ImageUrl ?? string.Empty);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [IsRockBandOwner]
        public IActionResult UpdateGenres(int bandId, int[] selectedGenreIds)
        {
            _rockBandsService.UpdateBandGenres(bandId, selectedGenreIds);
            return RedirectToAction(nameof(Index), new { editBandId = bandId });
        }
    }
}

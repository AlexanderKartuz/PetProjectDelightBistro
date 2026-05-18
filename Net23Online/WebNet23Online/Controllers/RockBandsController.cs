using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Enums;
using WebNet23Online.Models.RockBands;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class RockBandsController : Controller
    {
        private readonly IRockBandsService _rockBandsService;
        private readonly IAuthService _authService;

        public RockBandsController(IRockBandsService rockBandsService, IAuthService authService)
        {
            _rockBandsService = rockBandsService;
            _authService = authService;
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
            var viewModel = new RockBandsIndexViewModel
            {
                IsUserAuth = isAuth,
                CanEditRockBandGenres = isAuth && _authService.GetRole() == UserRole.RockBandOwner,
                Bands = _rockBandsService.GetBands(selectedGenreIds),
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
                var startViewModel = new RockBandsIndexViewModel
                {
                    IsUserAuth = isAuth,
                    CanEditRockBandGenres = isAuth && _authService.GetRole() == UserRole.RockBandOwner,
                    Bands = _rockBandsService.GetBands(Array.Empty<int>()),
                    Genres = genres,
                    SelectedGenreIds = Array.Empty<int>(),
                    EditBandId = null,
                    BandBlock = band,
                };
                return View(startViewModel);
            }

            var createdByUserId = _authService.GetUserId();
            _rockBandsService.AddBand(band, createdByUserId);
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

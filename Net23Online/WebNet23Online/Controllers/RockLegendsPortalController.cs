using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.DataModels;
using WebNet23Online.Data.Enums;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.RockLegendsPortal;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class RockLegendsPortalController : Controller
    {
        private readonly IRockLegendsPick _rockService;
        private readonly IRockLegendsRepository _rockLegendsRepository;
        private readonly IRockLegendsGenresRepository _genreRepository;
        private readonly IAuthService _authService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RockLegendsPortalController(
            IRockLegendsPick rockService,
            IRockLegendsRepository rockLegendsRepository,
            IRockLegendsGenresRepository genreRepository,
            IAuthService authService,
            IWebHostEnvironment webHostEnvironment)
        {
            _rockService = rockService;
            _rockLegendsRepository = rockLegendsRepository;
            _genreRepository = genreRepository;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Authorize]
        public IActionResult SortByGenre()
        {
            var genresDb = _genreRepository.GetAllWithGroups();
            var bandsDb = _rockLegendsRepository.GetAll();

            bool isRockModerator = _authService.AtLeastModerator();

            var viewModel = new SortByGenreViewModel
            {
                Genres = genresDb.Select(g => new RockLegendsGenreItemViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    CoverUrl = g.CoverUrl ?? "/images/rock-legends-portal/default.jpg",
                    BandNames = g.Groups.Select(b => b.GroupNames).ToList()
                }).ToList(),

                Bands = bandsDb.Select(x => new SelectListItem
                {
                    Text = x.GroupNames,
                    Value = x.Id.ToString()
                }).ToList(),

                IsCurrentUserAdmin = isRockModerator,

                AdminSqlStats = isRockModerator
                    ? _genreRepository.GetGenreStatsSql()
                    : new List<RockLegendsGenreStatsDataModel>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [IsRockLegendsModerator]
        public IActionResult DeleteGenre(int id)
        {
            var genre = _genreRepository.Get(id);

            if (genre != null)
            {
                _genreRepository.Remove(genre);
            }

            return RedirectToAction("SortByGenre");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddGenre()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddGenre(CreateGenreViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var genre = new RockLegendsGenres
            {
                Name = viewModel.Name,
                CoverUrl = viewModel.CoverUrl ?? "/images/rock-legends-portal/default.jpg"
            };

            _genreRepository.Add(genre);

            if (viewModel.Image != null)
            {
                var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
                var pathToFolder = Path.Combine("images", "rock-legends-portal");
                var fileName = $"genre-{genre.Id}.jpg";

                var absoluteFolderPath = Path.Combine(pathToWwwRootFolder, pathToFolder);
                if (!Directory.Exists(absoluteFolderPath))
                {
                    Directory.CreateDirectory(absoluteFolderPath);
                }
                var fullPath = Path.Combine(absoluteFolderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    viewModel.Image.CopyTo(stream);
                }

                genre.CoverUrl = $"/images/rock-legends-portal/{fileName}";
                _genreRepository.Update(genre);
            }

            return RedirectToAction("SortByGenre");
        }

        [HttpPost]
        [Authorize]
        public IActionResult LinkGroupToGenre(SortByGenreViewModel viewModel)
        {
            var band = _rockLegendsRepository.GetById(viewModel.SelectedBandId);
            if (band != null)
            {
                band.RockLegendsGenresId = viewModel.SelectedGenreId;
                _rockLegendsRepository.Update(band);
            }
            return RedirectToAction("SortByGenre");
        }

        [HttpGet]
        public IActionResult Index() => View();
        [HttpPost]
        [Authorize]
        public IActionResult Index(RockLegendsPortalViewModel viewModel)
        {
            if (viewModel.SelectedBandId != 0)
            {
                var targetBand = _rockLegendsRepository.GetById(viewModel.SelectedBandId);
                if (targetBand != null)
                {
                    targetBand.Likes++;
                    _rockLegendsRepository.Update(targetBand);
                    return RedirectToAction("Details", new { id = targetBand.Id });
                }
            }
            return View();
        }

        public IActionResult Details(int id)
        {
            var targetBand = _rockLegendsRepository.GetById(id);
            if (targetBand == null) return NotFound();
            var model = _rockService.GetBandDetails(id, targetBand);
            return View(model);
        }
    }
}
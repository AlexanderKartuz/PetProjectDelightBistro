using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Data.Enums;
using System.Globalization;
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

        public RockLegendsPortalController(
            IRockLegendsPick rockService,
            IRockLegendsRepository rockLegendsRepository,
            IRockLegendsGenresRepository genreRepository,
            IAuthService authService) 
        {
            _rockService = rockService;
            _rockLegendsRepository = rockLegendsRepository;
            _genreRepository = genreRepository;
            _authService = authService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult SortByGenre()
        {
            var genresDb = _genreRepository.GetAllWithGroups();
            var bandsDb = _rockLegendsRepository.GetAll();
            var currentUser = _authService.GetUser();

            var viewModel = new SortByGenreViewModel
            {
                // Передаем только нужные данные (модельки, а не Data)
                Genres = genresDb.Select(g => new RockLegendsGenreItemViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    BandNames = g.Groups.Select(b => b.GroupNames).ToList()
                }).ToList(),

                Bands = bandsDb.Select(x => new SelectListItem
                {
                    Text = x.GroupNames,
                    Value = x.Id.ToString()
                }).ToList(),

                IsCurrentUserAdmin = currentUser != null && currentUser.Role == UserRole.Admin
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
        public IActionResult AddGenre() => View();

        [HttpPost]
        [Authorize]
        public IActionResult AddGenre(CreateGenreViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var genre = new RockLegendsGenres { Name = viewModel.Name, CoverUrl = viewModel.CoverUrl };
            _genreRepository.Add(genre);
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
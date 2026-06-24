using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Controllers.CustomAuthAttribute;
using WebNet23Online.Hubs;
using WebNet23Online.Models.AnimalWorld;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers
{
    public class AnimalWorldController : Controller
    {
        private IAnimalWorldService _animalWorldService;
        private IHubContext<AnimalWorldHub, IAnimalWorldHub> _animalWorldHub;

        public AnimalWorldController(IAnimalWorldService animalWorldService, IHubContext<AnimalWorldHub, IAnimalWorldHub> animalWorldHub)
        {
            _animalWorldService = animalWorldService;
            _animalWorldHub = animalWorldHub;
        }

        public async Task<IActionResult> Index()
        {
            var startInfo = await _animalWorldService.GetStartInfo();
            return View(startInfo);
        }

        [Authorize]
        [IsModerator]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public IActionResult AddZoo()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public IActionResult AddZoo(ZooViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (_animalWorldService.AddZoo(viewModel))
            {
                return RedirectToAction("Add");
            }
            
            return View();
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public IActionResult AddFamily()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public IActionResult AddFamily(AnimalFamilyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (_animalWorldService.AddAnimalFamily(viewModel))
            {
                return RedirectToAction("Add");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public IActionResult AddSpecies()
        {
            return View(_animalWorldService.GetAnimalSpeciesPageInfo());
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public IActionResult AddSpecies(AnimalSpeciesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AnimalFamilyNames = _animalWorldService.GetAnimalSpeciesPageInfo().AnimalFamilyNames;
                return View(viewModel);
            }

            if (_animalWorldService.AddAnimalSpecies(viewModel))
            {
                return RedirectToAction("Add");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        [IsModerator]
        public IActionResult BindZooAndAnimalSpecies()
        {
            return View(_animalWorldService.GetBingZooAndAnimalSpeciesInfo());
        }

        [HttpPost]
        [Authorize]
        [IsModerator]
        public IActionResult BindZooAndAnimalSpecies(BindZooWithAnimalSpeciesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var bindings = _animalWorldService.GetBingZooAndAnimalSpeciesInfo();
                viewModel.Zoos = bindings.Zoos;
                viewModel.AnimalSpecies = bindings.AnimalSpecies;
                return View(viewModel);
            }

            if (_animalWorldService.BindZooWithAnimalSpecies(viewModel.ZooId, viewModel.AnimalSpeciesId))
            {
                var zooName = _animalWorldService.GetZooName(viewModel.ZooId);
                var animalSpeciesName = _animalWorldService.GetAnimalSpeciesName(viewModel.AnimalSpeciesId);
                _animalWorldHub.Clients.All.NewAnimalInZooAppeared(zooName, animalSpeciesName);
                return RedirectToAction("Index");
            }

            return View();
        }

        [Authorize]
        public IActionResult Zoos()
        {
            return View(_animalWorldService.GetAllZoos());
        }

        public IActionResult InterestingFacts()
        {
            return View();
        }
    }
}

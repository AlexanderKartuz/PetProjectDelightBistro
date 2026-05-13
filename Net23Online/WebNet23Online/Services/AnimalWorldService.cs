using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;
using WebNet23Online.Models.AnimalWorld;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class AnimalWorldService : IAnimalWorldService
    {
        private IZooRepository _zooRepository;
        private IAnimalFamilyRepository _animalFamilyRepository;
        private IAnimalSpeciesRepository _animalSpeciesRepository;
        private IAnimalWorldMapper _animalWorldMapper;
        private IAuthService _authService;
        private IWebHostEnvironment _webHostEnvironment;

        public AnimalWorldService(IZooRepository zooRepository, IAnimalFamilyRepository animalFamilyRepository, IAnimalSpeciesRepository animalSpeciesRepository, IAnimalWorldMapper animalWorldMapper, IAuthService authService, IWebHostEnvironment webHostEnvironment)
        {
            _zooRepository = zooRepository;
            _animalFamilyRepository = animalFamilyRepository;
            _animalSpeciesRepository = animalSpeciesRepository;
            _animalWorldMapper = animalWorldMapper;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
        }

        public StartPageAnimalWorldInfoViewModel GetStartInfo()
        {
            var zoos = _animalWorldMapper.FromZooDataToZooViewModel(_zooRepository.GetRandomElements());
            var animalFamilies = _animalWorldMapper.FromAnimalFamilyDataToAnimalFamilyViewModel(_animalFamilyRepository.GetRandomElements());
            var animalSpecies = _animalWorldMapper.FromAnimalSpeciesDataToAnimalSpeciesViewModel(_animalSpeciesRepository.GetRandomElements());
            foreach (var animal in animalSpecies)
            {
                if (string.IsNullOrEmpty(animal.Url))
                {
                    animal.Url = "/images/animal-world/default.jpg";
                }
            }

            var startPageInfo = new StartPageAnimalWorldInfoViewModel
            {
                Zoos = zoos,
                AnimalFamilies = animalFamilies,
                AnimalSpecies = animalSpecies,
            };
            return startPageInfo;
        }

        public AnimalSpeciesViewModel GetAnimalSpeciesPageInfo()
        {
            var animalFamilies = GetAnimalFamilies();
            var animalFamilyListItems = new List<SelectListItem>();
            animalFamilyListItems.AddRange(animalFamilies.Select(animalFamily => new SelectListItem
            {
                Text = animalFamily.AnimalFamilyName,
                Value = animalFamily.Id.ToString()
            }));
            var viewModel = new AnimalSpeciesViewModel
            {
                AnimalFamilyNames = animalFamilyListItems
            };

            return viewModel;
        }

        private List<AnimalFamilyData> GetAnimalFamilies()
        {
            return _animalFamilyRepository.GetAll();
        }

        public BindZooWithAnimalSpeciesViewModel GetBingZooAndAnimalSpeciesInfo()
        {
            var zoos = _zooRepository.GetAll();
            var zoosListItems = new List<SelectListItem>();
            zoosListItems.AddRange(zoos.Select(zoos => new SelectListItem
            {
                Text = zoos.ZooName,
                Value = zoos.Id.ToString()
            }));
            var animalSpecies = _animalSpeciesRepository.GetAll();
            var animalSpeciesListItems = new List<SelectListItem>();
            animalSpeciesListItems.AddRange(animalSpecies.Select(animalSpecies => new SelectListItem
            {
                Text = animalSpecies.AnimalSpeciesName,
                Value = animalSpecies.Id.ToString()
            }));
            var bindModel = new BindZooWithAnimalSpeciesViewModel
            {
                Zoos = zoosListItems,
                AnimalSpecies = animalSpeciesListItems
            };
            return bindModel;
        }

        public bool AddZoo(ZooViewModel viewModel)
        {
            var user = _authService.GetUser();
            var zooData = new ZooData
            {
                ZooName = viewModel.ZooName,
                Address = viewModel.Address,
                Description = viewModel.Description,
                Creator = user
            };
            _zooRepository.Add(zooData);
            return true;
        }

        public bool AddAnimalFamily(AnimalFamilyViewModel viewModel)
        {
            var user = _authService.GetUser();
            var animalFamilyData = new AnimalFamilyData
            {
                AnimalFamilyName = viewModel.AnimalFamilyName,
                Description = viewModel.Description,
                Creator = user
            };
            _animalFamilyRepository.Add(animalFamilyData);
            return true;
        }

        public bool AddAnimalSpecies(AnimalSpeciesViewModel viewModel)
        {
            var user = _authService.GetUser();
            var animalFamily = _animalFamilyRepository.Get(viewModel.AnimalFamilyId);
            var url = "/images/animal-world/default.jpg";
            if (viewModel.AnimalSpeciesImage != null)
            {
                var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
                var pathToFolder = "images\\animal-world";
                var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}-animal-{user.Name}.jpg";
                url = $"/images/animal-world/{fileName}";
                var path = Path.Combine(pathToWwwRootFolder, pathToFolder, fileName);
                using (var animalSpeciesImage = new FileStream(path, FileMode.Create))
                {
                    viewModel.AnimalSpeciesImage.CopyTo(animalSpeciesImage);
                }
            }

            var animalSpeciesData = new AnimalSpeciesData
            {
                AnimalSpeciesName = viewModel.AnimalSpeciesName,
                AnimalSpeciesUrl = url,
                NativeRange = viewModel.NativeRange,
                Description = viewModel.Description,
                AnimalFamily = animalFamily,
                Creator = user
            };
            _animalSpeciesRepository.Add(animalSpeciesData);
            //animalSpeciesData = _animalSpeciesRepository.GetElementByName(viewModel.AnimalSpeciesName);
            //animalFamily.Species.Add(animalSpeciesData);
            return true;
        }

        public bool BindZooWithAnimalSpecies(int zooId, int animalSpeciesId)
        {
            _zooRepository.AddAnimalSpecies(zooId, animalSpeciesId);
            return true;
        }

        public List<ZooViewModel> GetAllZoos()
        {
            return _animalWorldMapper.FromZooDataToZooViewModel(_zooRepository.GetAllWithAnimalSpecies());
        }
    }
}

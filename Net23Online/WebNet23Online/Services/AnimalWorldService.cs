using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Models.AnimalWorld;
using WebNet23Online.Data.Repositories.Interfaces.AnimalWorld;
using WebNet23Online.Models.AnimalWorld;
using WebNet23Online.Models.DTOs;
using WebNet23Online.Services.Apis;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class AnimalWorldService : IAnimalWorldService
    {
        public const string DEFAULT_URL = "/images/animal-world/default.jpg";
        private IZooRepository _zooRepository;
        private IAnimalFamilyRepository _animalFamilyRepository;
        private IAnimalSpeciesRepository _animalSpeciesRepository;
        private IPromotionRepository _promotionRepository;
        private IAnimalWorldMapper _animalWorldMapper;
        private IAuthService _authService;
        private IWebHostEnvironment _webHostEnvironment;
        private AnimalWorldRandomAnimalApi _randomAnimalApi;
        private const int RANDOM_ANIMAL_IMAGE_COUNT = 9;
        private Random _random;

        public AnimalWorldService(IZooRepository zooRepository, IAnimalFamilyRepository animalFamilyRepository, IAnimalSpeciesRepository animalSpeciesRepository, IAnimalWorldMapper animalWorldMapper, IAuthService authService, IWebHostEnvironment webHostEnvironment, AnimalWorldRandomAnimalApi randomAnimalApi, IPromotionRepository promotionRepository)
        {
            _zooRepository = zooRepository;
            _animalFamilyRepository = animalFamilyRepository;
            _animalSpeciesRepository = animalSpeciesRepository;
            _animalWorldMapper = animalWorldMapper;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
            _randomAnimalApi = randomAnimalApi;
            _random = new Random();
            _promotionRepository = promotionRepository;
        }

        public async Task<StartPageAnimalWorldInfoViewModel> GetStartInfo()
        {
            var animalFamilies = _animalWorldMapper.FromAnimalFamilyDataToAnimalFamilyViewModel(_animalFamilyRepository.GetRandomElements());
            var animalSpecies = _animalWorldMapper.FromAnimalSpeciesDataToAnimalSpeciesViewModel(_animalSpeciesRepository.GetRandomElements());
            var animalImages = await GetRandomAnimalImages();
            var startPageInfo = new StartPageAnimalWorldInfoViewModel
            {
                AnimalFamilies = animalFamilies,
                AnimalSpecies = animalSpecies,
                RandomAnimals = animalImages,
            };
            return startPageInfo;
        }

        private async Task<List<AnimalWorldRandomAnimalDto>> GetRandomAnimalImages()
        {
            var animalSpecies = await _randomAnimalApi.GetAnimalSpecies();
            var tasks = new List<Task<AnimalWorldRandomAnimalDto>>();
            for ( var i = 0; i < RANDOM_ANIMAL_IMAGE_COUNT; i++)
            {
                var index = _random.Next(animalSpecies.Count);
                var selectedType = animalSpecies[index];
                tasks.Add(_randomAnimalApi.GetRandomAnimal(selectedType));
            }

            var animalWorldRandomAnimals = await Task.WhenAll(tasks);
            return animalWorldRandomAnimals.ToList();
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

        public PromotionViewModel GetPromotionsPageInfo()
        {
            var zoos = _zooRepository.GetAll();
            var zooListItems = new List<SelectListItem>();
            zooListItems.AddRange(zoos.Select(zoo => new SelectListItem
            {
                Text = zoo.ZooName,
                Value = zoo.Id.ToString()
            }));
            var viewModel = new PromotionViewModel
            {
                Zoos = zooListItems
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
            var url = DEFAULT_URL;
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
            return true;
        }

        public bool AddPromotion(PromotionViewModel viewModel)
        {
            var user = _authService.GetUser();
            var promotionData = new PromotionData
            {
                PromotionName = viewModel.PromotionName,
                Description = viewModel.Description,
                EndDate = viewModel.EndDate,
                CreatorId = user.Id,
                VenueId = viewModel.ZooId,
            };
            _promotionRepository.Add(promotionData);
            return true;
        }

        public bool BindZooWithAnimalSpecies(int zooId, int animalSpeciesId)
        {
            _zooRepository.AddAnimalSpecies(zooId, animalSpeciesId);
            return true;
        }

        public List<ZooViewModel> GetAllZoos()
        {
            var zoos = _animalWorldMapper.FromZooDataToZooViewModel(_zooRepository.GetAll());
            foreach (var zoo in zoos)
            {
                zoo.AnimalFamilies = _zooRepository.GetZooAnimalFamilies(zoo.Id);
            }

            return zoos;
        }

        public string GetZooName(int zooId)
        {
            return _zooRepository.Get(zooId).ZooName;
        }

        public string GetAnimalSpeciesName(int animalSpeciesId)
        {
            return _animalSpeciesRepository.Get(animalSpeciesId).AnimalSpeciesName;
        }

        public List<PromotionViewModel> GetAllPromotions()
        {
            return _animalWorldMapper.FromPromotionDataToPromotionViewModel(_promotionRepository.GetAllWithZoos());
        }
    }
}

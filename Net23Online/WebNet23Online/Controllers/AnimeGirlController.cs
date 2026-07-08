using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Data.HelperModels.SteamPagination;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Hubs;
using WebNet23Online.Models.AnimeGirl;
using WebNet23Online.Services.Apis;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers;

public class AnimeGirlController : Controller
{
    private const int HeroesDefaultPageSize = 3;
    private static readonly int[] AllowedHeroesPageSizes = { 0, 1, 2, 3, 6, 12 };

    private IAnimeGirlService _animeGirlService;
    private IAnimeGirlRepository _animeGirlRepository;
    private IAnimeRepository _animeRepository;
    private IAuthService _authService;
    private IWebHostEnvironment _webHostEnvironment;
    private IHubContext<AnimeHub, IAnimeHub> _animeHub;
    private JokeApi _jokeApi;
    private WaifuApi _waifuApi;
    private CatApi _catApi;

    public AnimeGirlController(IAnimeGirlService animeGirlGenerator,
        IAnimeGirlRepository animeGirlRepository,
        IAnimeRepository animeRepository,
        IAuthService authService,
        IWebHostEnvironment webHostEnvironment,
        IHubContext<AnimeHub, IAnimeHub> animeHub,
        JokeApi jokeApi,
        WaifuApi waifuApi,
        CatApi catApi)
    {
        _animeGirlService = animeGirlGenerator;
        _animeGirlRepository = animeGirlRepository;
        _animeRepository = animeRepository;
        _authService = authService;
        _webHostEnvironment = webHostEnvironment;
        _animeHub = animeHub;
        _jokeApi = jokeApi;
        _waifuApi = waifuApi;
        _catApi = catApi;
    }

    //    /AnimeGirl/Index
    public async Task<IActionResult> Index(int page = 1, int pageSize = HeroesDefaultPageSize)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (!AllowedHeroesPageSizes.Contains(pageSize))
        {
            pageSize = HeroesDefaultPageSize;
        }

        var heroesFilter = new AnimeGirlHeroesPaginationFilterViewModel
        {
            Page = page,
            PageSize = pageSize
        };

        var pagedHeroes = _animeGirlRepository.GetPagedIncludeAnime(page, pageSize);
        heroesFilter.Page = pagedHeroes.PageIndex;

        var allAnimeGirlDatas = _animeGirlRepository.GetAllIncludeAnime();
        var animeDatas = _animeRepository.GetAll();

        var viewModels = _animeGirlService.GenerateList(pagedHeroes.Items);
        var allViewModels = _animeGirlService.GenerateList(allAnimeGirlDatas);
        var animeViewModels = _animeGirlService.AnimeMap(animeDatas);

        var jokeDtoTask = _jokeApi.GetJoke();
        var waifuDtoTask = _waifuApi.GetWaifu();
        var catDtosTask = _catApi.GetCats(); 

        Task.WaitAll(jokeDtoTask, waifuDtoTask, catDtosTask);

        var jokeDto = jokeDtoTask.Result;
        var waifuDto = waifuDtoTask.Result;
        var catDtos = catDtosTask.Result;

        var mainViewModel = new MainIndexViewModel
        {
            AnimeGirls = viewModels,
            AllAnimeGirls = allViewModels,
            Animes = animeViewModels,
            HeroesFilter = heroesFilter,
            HeroesPagination = new PaginationMetadataViewModel
            {
                CurrentPage = pagedHeroes.PageIndex,
                PageSize = pageSize,
                TotalCount = pagedHeroes.TotalCount,
                TotalPages = pagedHeroes.TotalPages,
                HasPreviousPage = pagedHeroes.HasPreviousPage,
                HasNextPage = pagedHeroes.HasNextPage,
            },
            CanDeleteGirl = _authService.AtLeastModerator(),
            Cats = catDtos,
            Joke = jokeDto,
            Waifu = waifuDto
        };

        return View(mainViewModel);
    }

    [HttpGet]
    [Authorize]
    public IActionResult CreateGirl()
    {
        var viewModel = new CreateAnimeGirlViewModel
        {
            Animes = _animeGirlService.GetListItemsWithAnime()
        };

        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateGirl(CreateAnimeGirlViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Animes = _animeGirlService.GetListItemsWithAnime();
            return View(viewModel);
        }

        var animeGirlData = new AnimeGirlData
        {
            Description = viewModel.Description,
            Name = viewModel.Name,
            Url = viewModel.Url ?? "/images/anime-girl/default.jpg",
        };

        if (!_animeGirlRepository.IsNameFree(viewModel.Name))
        {
            return View(viewModel);
        }

        if (viewModel.AnimeId is not null
            && viewModel.AnimeId > 0)
        {
            var anime = _animeRepository.Get(viewModel.AnimeId.Value);
            animeGirlData.Animes.Add(anime!);
        }

        _animeGirlRepository.Add(animeGirlData);

        if (viewModel.Image != null)
        {
            var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
            var pathToFolder = "images\\anime-girl";
            var fileName = $"girl-{animeGirlData.Id}.jpg";
            var path = Path.Combine(pathToWwwRootFolder, pathToFolder, fileName);
            using (var animeGirlFile = new FileStream(path, FileMode.Create))
            {
                viewModel.Image.CopyTo(animeGirlFile);
            }

            animeGirlData.Url = $"/images/anime-girl/{fileName}";
            _animeGirlRepository.Update(animeGirlData);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize]
    public IActionResult CreateAnime()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateAnime(CreateAnimeViewModel viewModel)
    {
        var anime = new AnimeData
        {
            Name = viewModel.Name,
            CoverUrl = viewModel.CoverUrl
        };
        _animeRepository.Add(anime);

        _animeHub.Clients.All.NewAnimeCreated(viewModel.Name, viewModel.CoverUrl);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult LinkAnimeAndGirl(int animeId, int heroId)
    {
        _animeGirlRepository.Link(animeId, heroId);
        return RedirectToAction(nameof(Index));
    }

    //    /AnimeGirl/Handmade
    public IActionResult Handmade()
    {
        var minutes = DateTime.Now.Minute;
        var second = DateTime.Now.Second;
        var name = "Ivan";

        var viewModel = new HandMadeViewModel
        {
            Minutes = minutes,
            Seconds = second,
            Name = name
        };

        return View(viewModel);
    }

    public IActionResult Delete(int id)
    {
        _animeGirlRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult TableData(string? sortBy = null, 
        string? direction = "asc", 
        string? sortType = "",
        string? sortValue = "")
    {
        var animeGirlDatas = _animeGirlRepository.GetAllWithExpression(sortBy, 
            direction, 
            sortType, 
            sortValue);
        var viewModels = _animeGirlService.GenerateList(animeGirlDatas);
        return View(viewModels);
    }

    public IActionResult AnimeTableData(string? sortBy = null, string? direction = "asc")
    {
        var animeDatas = _animeRepository.GetAllWithExpression(sortBy, direction, "", "");
        var viewModels = _animeGirlService.AnimeMap(animeDatas);
        return View(viewModels);
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebNet23Online.Data.HelperModels;
using WebNet23Online.Data.Models.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Models.Steam;

using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.Steam;

namespace WebNet23Online.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IGameGenreRepository _gameGenreRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;

        public CatalogService(IGameRepository gameRepository,
            IPublisherRepository publisherRepository,
            IGameGenreRepository gameGenreRepository,
            IHttpContextAccessor httpContextAccessor,
            IAuthService authService)
        {
            _gameRepository = gameRepository;
            _publisherRepository = publisherRepository;
            _gameGenreRepository = gameGenreRepository;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        public SteamHomeViewModel GetGamesForHomePage()
        {
            var viewModel = new SteamHomeViewModel
            {
                Featured = _gameRepository.GetFeaturedForHomePage()
                   .Select(g => new SteamGameViewModel
                   {
                       Id = g.Id,
                       Title = g.Title,
                       Description = g.Description,
                       ImageUrl = g.ImageUrl,
                       Price = g.Price,
                       Genres = g.GameGenres.Select(genre => genre.Name).ToList()
                   })
                   .ToList(),

                SpecialOffers = _gameRepository.GetSpecialOffersForHomePage()
                   .Select(g => new SteamGameViewModel
                   {
                       Id = g.Id,
                       Title = g.Title,
                       Description = g.Description,
                       ImageUrl = g.ImageUrl,
                       Price = g.Price,
                       Genres = g.GameGenres.Select(genre => genre.Name).ToList()
                   })
                   .ToList()
            };

            return viewModel;
        }

        public CatalogViewModel GetCatalog(CatalogFilterViewModel filter = null)
        {
            filter ??= new CatalogFilterViewModel();

            var genres = _gameGenreRepository.GetAll();

            var repositoryFilter = new GameFilter
            {
                GenreId = filter.GenreId,
                MaxPrice = filter.MaxPrice
            };

            var games = _gameRepository.GetFilteredWithGenres(repositoryFilter);

            return new CatalogViewModel
            {
                Filter = filter,
                Games = games
                    .Select(g => new SteamGameViewModel
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Description = g.Description,
                        ImageUrl = g.ImageUrl,
                        Price = g.Price,
                        Genres = g.GameGenres.Select(gg => gg.Name).ToList(),
                    })
                    .ToList(),
                GameGenres = GetListItemsWithGameGenres()
            };
        }

        public void AddGame(AddGameViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel), "Game data cannot be null");
            }

            var currentUserId = _authService.GetUserId();

            var gameEntity = new GameData
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                ImageUrl = viewModel.ImageUrl,
                Price = viewModel.Price,
                GameGenres = new List<GameGenreData>(),
                PublisherId = viewModel.PublisherId,
                CreatedByUserId = currentUserId,
                ModifiedByUserId = currentUserId,
                CreatedAt = DateTime.UtcNow
            };

            if (viewModel.SelectedGenreIds != null && viewModel.SelectedGenreIds.Any())
            {
                var genres = _gameGenreRepository.GetByIds(viewModel.SelectedGenreIds);
                foreach (var genre in genres)
                {
                    gameEntity.GameGenres.Add(genre);
                }
            }

            _gameRepository.Add(gameEntity);
        }

        public List<SelectListItem> GetListItemsWithPublishers()
        {
            var publishers = _publisherRepository.GetAll();
            var publisherListItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "SelectPublisher",
                    Value = ""
                }
            };

            publisherListItems.AddRange(publishers.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }));

            return publisherListItems;
        }

        public List<SelectListItem> GetListItemsWithGameGenres()
        {
            var gameGenres = _gameGenreRepository.GetAll();
            var gameGenresListItems = new List<SelectListItem>();

            gameGenresListItems.AddRange(gameGenres.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }));

            return gameGenresListItems;
        }

        public List<PublisherData> GetPublishers()
        {
            var publishers = _publisherRepository.GetAll();
            return publishers;
        }

        public List<GameGenreData> GetGameGenres()
        {
            var gameGenres = _gameGenreRepository.GetAll();
            return gameGenres;
        }

        public GameData GetGameDetails(int id)
        {
            var game = _gameRepository.GetGameWithPublisherAndGenres(id);
            return game;
        }

        public void UpdateGame(EditGameViewModel viewModel)
        {
            var game = _gameRepository.GetGameWithPublisherAndGenres(viewModel.Id);

            if (game == null)
            {
                throw new ArgumentException($"Game not found");
            }

            var currentUserId = _authService.GetUserId();

            game.Title = viewModel.Title;
            game.Description = viewModel.Description;
            game.ImageUrl = viewModel.ImageUrl;
            game.Price = viewModel.Price;
            game.PublisherId = viewModel.PublisherId;
            game.ModifiedByUserId = currentUserId;
            game.ModifiedAt = DateTime.UtcNow;

            if (viewModel.SelectedGenreIds != null && viewModel.SelectedGenreIds.Any())
            {
                var genres = _gameGenreRepository.GetByIds(viewModel.SelectedGenreIds);
                foreach (var genre in genres)
                {
                    game.GameGenres.Add(genre);
                }
            }

            _gameRepository.Update(game);
        }

        public void DeleteGame(int id)
        {
            var game = _gameRepository.Get(id);

            if (game == null)
            {
                throw new ArgumentException($"Game not found");
            }

            _gameRepository.Remove(game);
        }

        public bool IsUserCreatorOfTheGame(int userId, int gameId)
        {
            var game = _gameRepository.Get(gameId);

            if (game == null)
            {
                throw new ArgumentException($"Game not found");
            }

            return userId == game.CreatedByUserId;
        }
    }
}

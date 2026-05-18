using NAudio.Codecs;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.RockBands;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class RockBandsService : IRockBandsService
    {
        private IRockBandsRepository _rockBandsRepository;
        private IGenreOfRockBandsRepository _genreOfRockBandsRepository;
        private IWebHostEnvironment _webHostEnvironment;

        public RockBandsService(
            IRockBandsRepository rockBandsRepository,
            IGenreOfRockBandsRepository genreOfRockBandsRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _rockBandsRepository = rockBandsRepository;
            _genreOfRockBandsRepository = genreOfRockBandsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<BandBlockViewModel> GetBands(int[]? genreIds = null)
        {
            var bandsBlockViewData = (genreIds != null && genreIds.Length > 0)
                ? _rockBandsRepository.GetByGenreIdsWithGenres(genreIds)
                : _rockBandsRepository.GetAllWithGenres();

            return bandsBlockViewData
                   .OrderBy(b => b.Id)
                   .Select(b => new BandBlockViewModel
                   {
                       Id = b.Id,
                       Name = b.Name,
                       Description = b.Description,
                       ImageUrl = string.IsNullOrWhiteSpace(b.Url) ? null : b.Url,
                       CreatedByUserName = b.CreatedByUser != null ? b.CreatedByUser.Name : null,
                       GenreIds = b.RockBandGenres.Select(bg => bg.GenreId).ToList(),
                       Genres = b.RockBandGenres
                            .Select(bg => bg.Genre.Name)
                            .OrderBy(x => x)
                            .ToList(),
                   })
                   .ToList();
        }

        public List<GenreFilterItemViewModel> GetGenres()
        {
            return _genreOfRockBandsRepository
                .GetAll()
                .OrderBy(g => g.Name)
                .Select(g => new GenreFilterItemViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsSelected = false,
                })
                .ToList();
        }

        public void AddBand(BandBlockViewModel viewModel, int createdByUserId)
        {
            if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.Name))
            {
                return;
            }

            var genreIds = (viewModel.SelectedGenreIds ?? Array.Empty<int>())
                .Where(x => x > 0)
                .Distinct()
                .ToArray();

            if (viewModel.PhotoOfTheBand != null)
            {
                var pathToWwwRootFolder = _webHostEnvironment.WebRootPath;
                var pathToFolder = "images\\rock-bands";
                var fileName = $"band-{viewModel.Name.Trim()}.jpg";
                var path = Path.Combine(pathToWwwRootFolder, pathToFolder, fileName);
                using (var rockBandPhotoFile = new FileStream(path, FileMode.Create))
                {
                    viewModel.PhotoOfTheBand.CopyTo(rockBandPhotoFile);
                }
                viewModel.ImageUrl = $"/images/rock-bands/{fileName}";
            }
            
            var newBand = new RockBandsData
            {
                Name = viewModel.Name.Trim(),
                Description = viewModel.Description?.Trim() ?? string.Empty,
                Url = string.IsNullOrWhiteSpace(viewModel.ImageUrl)
                    ? string.Empty
                    : viewModel.ImageUrl.Trim(),
                CreatedByUserId = createdByUserId > 0 ? createdByUserId : null,
                RockBandGenres = genreIds
                    .Select(id => new RockBandGenreData { GenreId = id })
                    .ToList(),
            };

            _rockBandsRepository.Add(newBand);
        }

        public void UpdateBandGenres(int bandId, int[] genreIds)
        {
            if (bandId <= 0)
            {
                return;
            }

            _rockBandsRepository.UpdateBandGenres(bandId, genreIds);
        }
    }
}

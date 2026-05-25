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
        private IRockBandLikeRepository _rockBandLikeRepository;
        private IGenreOfRockBandsRepository _genreOfRockBandsRepository;
        private IWebHostEnvironment _webHostEnvironment;

        public RockBandsService(
            IRockBandsRepository rockBandsRepository,
            IRockBandLikeRepository rockBandLikeRepository,
            IGenreOfRockBandsRepository genreOfRockBandsRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _rockBandsRepository = rockBandsRepository;
            _rockBandLikeRepository = rockBandLikeRepository;
            _genreOfRockBandsRepository = genreOfRockBandsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<BandBlockViewModel> GetBands(int[]? genreIds = null, int? currentUserId = null)
        {
            var bandsBlockViewData = (genreIds != null && genreIds.Length > 0)
                ? _rockBandsRepository.GetByGenreIdsWithGenres(genreIds)
                : _rockBandsRepository.GetAllWithGenres();

            var orderedBands = bandsBlockViewData.OrderBy(b => b.Id).ToList();
            var bandIds = orderedBands.Select(b => b.Id).ToList();
            var likedBandIds = currentUserId > 0
                ? _rockBandLikeRepository.GetLikedRockBandIds(currentUserId.Value, bandIds)
                : new HashSet<int>();

            return orderedBands
                   .Select(b => new BandBlockViewModel
                   {
                       Id = b.Id,
                       Name = b.Name,
                       Description = b.Description,
                       ImageUrl = string.IsNullOrWhiteSpace(b.Url) ? null : b.Url,
                       CreatedByUserName = b.CreatedByUser != null ? b.CreatedByUser.Name : null,
                       LikesCount = b.Likes,
                       IsLikedByCurrentUser = likedBandIds.Contains(b.Id),
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
                Likes = 0,
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

        public RockBandLikeResult? AddLike(int bandId, int userId)
        {
            if (bandId <= 0 || userId <= 0)
            {
                return null;
            }

            var band = _rockBandsRepository.Get(bandId);
            if (band == null)
            {
                return null;
            }

            if (!_rockBandLikeRepository.TryAddLike(userId, bandId))
            {
                return new RockBandLikeResult
                {
                    LikeCount = band.Likes,
                    Liked = true,
                    AlreadyLiked = true,
                };
            }

            band.Likes++;
            _rockBandsRepository.Update(band);

            return new RockBandLikeResult
            {
                LikeCount = band.Likes,
                Liked = true,
                AlreadyLiked = false,
            };
        }
    }
}

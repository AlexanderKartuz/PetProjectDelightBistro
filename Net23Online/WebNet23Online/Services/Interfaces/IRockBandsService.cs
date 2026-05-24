using WebNet23Online.Models.RockBands;

namespace WebNet23Online.Services.Interfaces
{
    public interface IRockBandsService
    {
        void AddBand(BandBlockViewModel viewModel, int createdByUserId);
        List<BandBlockViewModel> GetBands(int[]? genreIds = null);
        List<GenreFilterItemViewModel> GetGenres();
        void UpdateBandGenres(int bandId, int[] genreIds);
    }
}

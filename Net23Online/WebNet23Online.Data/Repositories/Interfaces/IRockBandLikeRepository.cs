namespace WebNet23Online.Data.Repositories.Interfaces
{
    public interface IRockBandLikeRepository
    {
        bool TryAddLike(int userId, int rockBandId);
        bool HasLike(int userId, int rockBandId);
        HashSet<int> GetLikedRockBandIds(int userId, IReadOnlyCollection<int> rockBandIds);
    }
}

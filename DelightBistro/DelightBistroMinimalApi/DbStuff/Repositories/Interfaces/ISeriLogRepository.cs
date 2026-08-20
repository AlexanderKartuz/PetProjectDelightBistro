namespace DelightBistroMinimalApi.DbStuff.Repositories.Interfaces
{
    public interface ISeriLogRepository
    {
        Task<int> CleanupAsync(CancellationToken ct);
    }
}
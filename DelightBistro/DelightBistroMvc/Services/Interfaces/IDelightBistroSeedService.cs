namespace DelightBistroMvc.Services.Interfaces
{
    public interface IDelightBistroSeedService
    {
        Task EnsureSeedAsync(CancellationToken cancellationToken = default);
    }
}

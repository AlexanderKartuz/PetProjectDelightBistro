namespace WebNet23Online.Services.Interfaces;

public interface IAnimeGirlChatService
{
    bool TryGetConnectionName(string connectionId, out string userName);

    void SetConnectionName(string connectionId, string userName);

    void RemoveConnection(string connectionId);

    Task JoinAsync(string connectionId, string userName);

    Task LeaveAsync(string connectionId);

    Task SendMessageAsync(string connectionId, string message);

    Task ShareCharactersAsync(string connectionId, IReadOnlyList<int> characterIds);

    bool IsInChat(string connectionId);
}

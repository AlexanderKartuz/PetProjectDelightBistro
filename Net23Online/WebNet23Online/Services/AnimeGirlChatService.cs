using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Hubs;
using WebNet23Online.Models.AnimeGirl;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services;

public class AnimeGirlChatService : IAnimeGirlChatService
{
    public const string ChatGroupName = "anime-girl-chat";
    private const int MaxSharedCharacters = 10;

    private readonly ConcurrentDictionary<string, string> _connectionNames = new();
    private readonly ConcurrentDictionary<string, byte> _chatMembers = new();

    private readonly IHubContext<AnimeHub, IAnimeHub> _hubContext;
    private readonly IServiceScopeFactory _scopeFactory;

    public AnimeGirlChatService(
        IHubContext<AnimeHub, IAnimeHub> hubContext,
        IServiceScopeFactory scopeFactory)
    {
        _hubContext = hubContext;
        _scopeFactory = scopeFactory;
    }

    public bool TryGetConnectionName(string connectionId, out string userName)
    {
        return _connectionNames.TryGetValue(connectionId, out userName!);
    }

    public void SetConnectionName(string connectionId, string userName)
    {
        _connectionNames[connectionId] = userName;
    }

    public void RemoveConnection(string connectionId)
    {
        _connectionNames.TryRemove(connectionId, out _);
        _chatMembers.TryRemove(connectionId, out _);
    }

    public async Task JoinAsync(string connectionId, string userName)
    {
        SetConnectionName(connectionId, userName);

        if (!_chatMembers.TryAdd(connectionId, 0))
        {
            await _hubContext.Clients.Client(connectionId).SetUserName(userName);
            return;
        }

        await _hubContext.Groups.AddToGroupAsync(connectionId, ChatGroupName);
        await _hubContext.Clients.Client(connectionId).SetUserName(userName);
        await _hubContext.Clients
            .GroupExcept(ChatGroupName, connectionId)
            .UserJoinedChat(userName);
    }

    public async Task LeaveAsync(string connectionId)
    {
        if (!_chatMembers.TryRemove(connectionId, out _))
        {
            return;
        }

        if (!_connectionNames.TryGetValue(connectionId, out var userName))
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, ChatGroupName);
            return;
        }

        await _hubContext.Clients
            .GroupExcept(ChatGroupName, connectionId)
            .UserLeftChat(userName);
        await _hubContext.Groups.RemoveFromGroupAsync(connectionId, ChatGroupName);
    }

    public async Task SendMessageAsync(string connectionId, string message)
    {
        if (!IsInChat(connectionId)
            || !_connectionNames.TryGetValue(connectionId, out var senderName))
        {
            return;
        }

        await _hubContext.Clients
            .Group(ChatGroupName)
            .ReceiveMessage(senderName, message);
    }

    public async Task ShareCharactersAsync(string connectionId, IReadOnlyList<int> characterIds)
    {
        if (!IsInChat(connectionId)
            || characterIds == null
            || characterIds.Count == 0
            || characterIds.Count > MaxSharedCharacters
            || !_connectionNames.TryGetValue(connectionId, out var senderName))
        {
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var animeGirlRepository = scope.ServiceProvider.GetRequiredService<IAnimeGirlRepository>();

        var characters = animeGirlRepository
            .GetByIds(characterIds)
            .Select(x => new SharedCharacterChatItem
            {
                Id = x.Id,
                Title = x.Name,
                Url = x.Url,
                Likes = x.Likes
            })
            .ToList();

        if (characters.Count == 0)
        {
            return;
        }

        await _hubContext.Clients
            .Group(ChatGroupName)
            .ReceiveSharedCharacters(senderName, characters);
    }

    public bool IsInChat(string connectionId)
    {
        return _chatMembers.ContainsKey(connectionId);
    }
}

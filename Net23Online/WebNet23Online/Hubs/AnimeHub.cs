using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Models.AnimeGirl;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Hubs;

public class AnimeHub : Hub<IAnimeHub>
{
    private readonly IAnimeGirlChatService _chatService;
    private readonly IAnimeGirlChatNicknameService _nicknameService;
    private readonly IAuthService _authService;

    public AnimeHub(
        IAnimeGirlChatService chatService,
        IAnimeGirlChatNicknameService nicknameService,
        IAuthService authService)
    {
        _chatService = chatService;
        _nicknameService = nicknameService;
        _authService = authService;
    }

    public async Task JoinChat()
    {
        var userName = ResolveUserName();
        await _chatService.JoinAsync(Context.ConnectionId, userName);
    }

    public async Task LeaveChat()
    {
        await _chatService.LeaveAsync(Context.ConnectionId);
    }

    public async Task SendMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await _chatService.SendMessageAsync(Context.ConnectionId, message.Trim());
    }

    public Task ShareCharacters(IReadOnlyList<int> characterIds)
    {
        return _chatService.ShareCharactersAsync(Context.ConnectionId, characterIds);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _chatService.LeaveAsync(Context.ConnectionId);
        _chatService.RemoveConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    private string ResolveUserName()
    {
        var connectionId = Context.ConnectionId;

        if (_chatService.TryGetConnectionName(connectionId, out var existingName))
        {
            return existingName;
        }

        if (_authService.IsAuthenticated())
        {
            var authName = _authService.GetUserName();
            if (!string.IsNullOrWhiteSpace(authName))
            {
                _chatService.SetConnectionName(connectionId, authName);
                return authName;
            }
        }

        var nickname = _nicknameService.Generate();
        _chatService.SetConnectionName(connectionId, nickname);
        return nickname;
    }
}

public interface IAnimeHub
{
    Task NewAnimeCreated(string animeName, string urlCover);
    Task ReceiveMessage(string senderName, string message);
    Task UserJoinedChat(string userName);
    Task UserLeftChat(string userName);
    Task SetUserName(string userName);
    Task ReceiveSharedCharacters(string senderName, IReadOnlyList<SharedCharacterChatItem> characters);
}

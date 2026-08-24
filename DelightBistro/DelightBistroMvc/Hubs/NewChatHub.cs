using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Models.DTOs.Chat;
using DelightBistroMvc.Services.Chat;
using DelightBistroMvc.Services.Chat.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace DelightBistroMvc.Hubs
{
    public class NewChatHub : Hub<INewChatHub>
    {
        public const string CHAT_GROUP_NAME = "new-chat";
        private readonly INewChatService _chatService;
        private readonly ChatPresenceService _chatPresenceService;

        public NewChatHub(INewChatService chatService, ChatPresenceService chatPresenceService)
        {
            _chatService = chatService;
            _chatPresenceService = chatPresenceService;
        }

        public async Task JoinChat()
        {
            var connectionId = Context.ConnectionId;
            var userName = _chatService.ResolveDisplayName(Context.User, connectionId);

            _chatPresenceService.Join(connectionId, userName);

            await Groups.AddToGroupAsync(connectionId, CHAT_GROUP_NAME);

            var history = await _chatService.GetRecentMessageAsync();
            var others = _chatPresenceService.GetOthers(connectionId);

            // МОжно ли Task.WhenAll()?
            await Clients.Caller.SetUserName(userName);
            await Task.WhenAll(
            Clients.Caller.ReceiveHistory(history),
            Clients.OthersInGroup(CHAT_GROUP_NAME).UserConnected(connectionId, userName),
            Clients.Caller.ConnectedUsers(others));
        }

        public async Task SendMessage(string text)
        {
            var connectionId = Context.ConnectionId;
            var userName = _chatPresenceService.GetUserName(connectionId)
                ?? _chatService.ResolveDisplayName(Context.User, connectionId);

            var userId = _chatService.TryGetUserId(Context.User);
            var messageDto = await _chatService.SaveMessageAsync(userName, text, userId);

            if (messageDto == null)
            {
                return;
            }

            await Clients.Group(CHAT_GROUP_NAME).ReceiveMessage(messageDto);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectedId = Context.ConnectionId;

            if (_chatPresenceService.Leave(connectedId, out var userName) && userName != null)
            {
                await Clients.OthersInGroup(CHAT_GROUP_NAME)
                    .UserDisconnected(connectedId, userName);
            }

            await Groups.RemoveFromGroupAsync(connectedId, CHAT_GROUP_NAME);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
using DelightBistroMvc.Hubs.Interfaces;
using DelightBistroMvc.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace DelightBistroMvc.Hubs
{
    public class DeligtBistroHub : Hub<IDeligtBistroHub>
    {
        // Общие пользователи
        private static ConcurrentDictionary<string, string> _chatUsers = new();
        // DTO
        public record ChatUser(string connectionId, string userName);

        public Task SendMessage(string senderName, string message)
        {
            return Clients.All.ReceiveMessage(GetUserName(), message);
        }

        public override async Task OnConnectedAsync()
        {
            var conectionId = Context.ConnectionId;
            var userName = GetUserName();

            await Clients.Caller.SetUserName(userName);
            //await Clients.All.UserConnected(connectionId, userName); // delete?

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            //var userName = GetUserName();

            if (_chatUsers.ContainsKey(connectionId))
            {
                var userName = _chatUsers[connectionId];
                await Clients.Others.UserDisconnected(connectionId, userName);
                _chatUsers.TryRemove(connectionId, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinChat()
        {
            var connectionId = Context.ConnectionId;
            var userName = GetUserName();
            _chatUsers[connectionId] = userName;

            var chatUsers = _chatUsers.Where(u => u.Key != connectionId)
                .Select(u => new ChatUser(u.Key, u.Value));

            await Clients.Caller.ConnectedUsers(chatUsers);
            await Clients.Others.UserConnected(connectionId, userName);
        }

        private string GetUserName()
        {
            var userName = Context.User?
                .FindFirstValue(AuthService.COOCKIE_NAME_KEY)
                ?? "Anonymous";
            return userName;
        }

    }
}

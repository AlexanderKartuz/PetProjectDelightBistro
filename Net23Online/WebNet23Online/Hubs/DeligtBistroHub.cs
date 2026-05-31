using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Services;

namespace WebNet23Online.Hubs
{
    public class DeligtBistroHub : Hub<IDeligtBistroHub>
    {
        // Общие пользователи
        private static Dictionary<string, string> _chatUsers = new();
        // DTO
        public record ChatUser(string connectionId, string userName);

        public Task SendMessage(string senderName, string message)
        {
            return Clients.All.ReceiveMessage(senderName, message);
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
                _chatUsers.Remove(connectionId);
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
        
        public string GetUserName()
        {
            var userName = Context.User?
                .FindFirstValue(AuthService.COOCKIE_NAME_KEY)
                ?? "Anonymous";
            return userName;
        }

    }
}

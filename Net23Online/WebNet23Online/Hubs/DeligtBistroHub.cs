using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Services;

namespace WebNet23Online.Hubs
{
    public class DeligtBistroHub : Hub<IDeligtBistroHub>
    {
        public Task SendMessage(string senderName, string message)
        {
            return Clients.All.ReceiveMessage(senderName, message);
        }

        public override async Task OnConnectedAsync()
        {
            // Get Current User
            var userName = Context.User?
                .FindFirstValue(AuthService.COOCKIE_NAME_KEY)
                ?? "Anonymous";

            var conectionId = Context.ConnectionId;

            await Clients.Caller.SetUserName(userName);
            await Clients.All.UserConnected(conectionId, userName);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User?
                .FindFirstValue(AuthService.COOCKIE_NAME_KEY)
                ?? "Anonymous";

            var connectionId = Context.ConnectionId;

            await Clients.All.UserDisconnected(connectionId, userName);

            await base.OnDisconnectedAsync(exception);

        }
    }
}

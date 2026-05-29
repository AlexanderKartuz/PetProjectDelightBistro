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

        public async override Task OnConnectedAsync()
        {
            // Get Current User
            var userName = Context.User?
                .FindFirstValue(AuthService.COOCKIE_NAME_KEY)
                ?? "Anonymous";

            await Clients.Caller.SetUserName(userName);

            await Clients.All.UserConnected(userName);

            await base.OnConnectedAsync();
        }
    }
}

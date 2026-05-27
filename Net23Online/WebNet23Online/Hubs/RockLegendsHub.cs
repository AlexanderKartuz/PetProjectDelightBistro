using Microsoft.AspNetCore.SignalR;

namespace WebNet23Online.Hubs
{
    public class RockLegendsHub : Hub<IRockLegendsHub>
    {
        public override Task OnConnectedAsync()
        {
            // connect
            return base.OnConnectedAsync();
        }
    }

    public interface IRockLegendsHub
    {
        Task NewGenreCreated(string genreName, string urlCover);
    }
}

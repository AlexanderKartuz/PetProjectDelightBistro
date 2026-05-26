using Microsoft.AspNetCore.SignalR;

namespace WebNet23Online.Hubs
{
    public class AnimeHub : Hub<IAnimeHub>
    {
        public override Task OnConnectedAsync()
        {
            // connect
            return base.OnConnectedAsync();
        }
    }

    public interface IAnimeHub
    {
        Task NewAnimeCreated(string animeName, string urlCover);
    }
}

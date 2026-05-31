using Microsoft.AspNetCore.SignalR;

namespace WebNet23Online.Hubs
{
    public class RockBandHub : Hub<IRockBandHub>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }

    public interface IRockBandHub
    {
        Task NewRockBandWasCreated(string rockBandName, string rockBandUrl);
    }
}

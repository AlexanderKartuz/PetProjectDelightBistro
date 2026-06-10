using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.LittleLemon;

namespace WebNet23Online.Hubs
{
    public class LittleLemonHub : Hub<ILittleLemonHub>
    {
        private readonly ILittleLemonChatService _chatService;
        private readonly IAuthService _authService;

        public LittleLemonHub(ILittleLemonChatService chatService, IAuthService authService)
        {
            _chatService = chatService;
            _authService = authService;
        }

        public override async Task OnConnectedAsync()
        {
            if (_authService.IsAuthenticated())
            {
                await _chatService.RegisterConnectionAsync(
                    Context.ConnectionId,
                    _authService.GetRole(),
                    _authService.GetUserId());
            }

            await base.OnConnectedAsync();
        }
    }
}

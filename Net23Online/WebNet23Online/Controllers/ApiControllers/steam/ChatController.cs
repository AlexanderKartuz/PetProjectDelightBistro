using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using WebNet23Online.Hubs;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Controllers.ApiControllers.steam
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IAuthService _authService;
        private IHubContext<SteamChatHub, ISteamChatHub> _steamChatHub;

        public ChatController(IAuthService authService, IHubContext<SteamChatHub, ISteamChatHub> steamChatHub)
        {
            _authService = authService;
            _steamChatHub = steamChatHub;
        }

        public IActionResult SendChatMessage(string message)
        {
            //var user = _authService.GetUser();
            var userName = _authService.GetUserName();
            _steamChatHub.Clients.All.SendChatMessage(userName, message);

            return Ok();
        }

    }
}

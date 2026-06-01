using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebNet23Online.Services.Interfaces.Steam;

namespace WebNet23Online.Controllers.ApiControllers.steam
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public IActionResult SendChatMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty");
            }
            
            _chatService.AddChatMessage(message);
            return Ok();
        }
    }
}

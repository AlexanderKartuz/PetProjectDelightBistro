using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNet23Online.Services.Interfaces.LittleLemon;

namespace WebNet23Online.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LittleLemonChatController : ControllerBase
    {
        private readonly ILittleLemonChatService _chatService;

        public LittleLemonChatController(ILittleLemonChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageToAdmin(string message)
        {
            await _chatService.SendMessageToAdminAsync(message);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageToUser(int targetUserId, string message)
        {
            await _chatService.SendMessageToUserAsync(targetUserId, message);
            return Ok();
        }
    }
}

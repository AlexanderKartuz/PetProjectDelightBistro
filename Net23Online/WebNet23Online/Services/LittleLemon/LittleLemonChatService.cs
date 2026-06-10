using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Data.Enums;
using WebNet23Online.Hubs;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.LittleLemon;

namespace WebNet23Online.Services.LittleLemon
{
    public class LittleLemonChatService : ILittleLemonChatService
    {
        private readonly string _userGroupPrefix = "little-lemon-user-";

        private readonly IAuthService _authService;
        private readonly IHubContext<LittleLemonHub, ILittleLemonHub> _hubContext;

        public LittleLemonChatService(
            IAuthService authService,
            IHubContext<LittleLemonHub, ILittleLemonHub> hubContext)
        {
            _authService = authService;
            _hubContext = hubContext;
        }

        public string AdminGroupName { get; } = "little-lemon-admins";

        public string GetUserGroupName(int userId) => $"{_userGroupPrefix}{userId}";

        public async Task RegisterConnectionAsync(string connectionId, UserRole role, int userId)
        {
            if (role == UserRole.Admin)
            {
                await _hubContext.Groups.AddToGroupAsync(connectionId, AdminGroupName);
            }
            else if (role == UserRole.User)
            {
                await _hubContext.Groups.AddToGroupAsync(connectionId, GetUserGroupName(userId));
            }
        }

        public async Task SendMessageToAdminAsync(string message)
        {
            var userId = _authService.GetUserId();
            var userName = _authService.GetUserName() ?? "Anonymous";

            await _hubContext.Clients.Group(AdminGroupName)
                .ReceivePrivateMessage(userId, userName, message);
            await _hubContext.Clients.Group(GetUserGroupName(userId))
                .ReceivePrivateMessage(userId, userName, message);
        }

        public async Task SendMessageToUserAsync(int targetUserId, string message)
        {
            var senderId = _authService.GetUserId();
            var senderName = _authService.GetUserName() ?? "Anonymous";

            await _hubContext.Clients.Group(GetUserGroupName(targetUserId))
                .ReceivePrivateMessage(senderId, senderName, message);
            await _hubContext.Clients.Group(AdminGroupName)
                .ReceivePrivateMessage(senderId, senderName, message);
        }
    }
}

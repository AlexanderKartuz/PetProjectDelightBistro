using Microsoft.AspNetCore.SignalR;
using WebNet23Online.Data.Models.Steam;
using WebNet23Online.Data.Repositories.Interfaces.Steam;
using WebNet23Online.Hubs;
using WebNet23Online.Hubs.Interfaces;
using WebNet23Online.Models.Steam;
using WebNet23Online.Services.Interfaces;
using WebNet23Online.Services.Interfaces.Steam;

namespace WebNet23Online.Services
{
    public class ChatService : IChatService
    {
        private readonly IAuthService _authService;
        private readonly ICommunityChatMessageRepository _communityChatMessageRepository;
        private IHubContext<SteamChatHub, ISteamChatHub> _steamChatHub;

        public ChatService(ICommunityChatMessageRepository communityChatMessageRepository, IAuthService authService, IHubContext<SteamChatHub, ISteamChatHub> steamChatHub)
        {
            _communityChatMessageRepository = communityChatMessageRepository;
            _authService = authService;
            _steamChatHub = steamChatHub;
        }

        public void AddChatMessage(string message)
        {
            var user = _authService.GetUser()!;
            var newMessage = new CommunityChatMessageData()
            {
                MessageText = message,
                CreatedAt = DateTime.UtcNow,
                CreatedByUser = user
            };

            _communityChatMessageRepository.Add(newMessage);

            _steamChatHub.Clients.All.SendChatMessage(user.Name, message, user.Id, newMessage.CreatedAt);
        }

        public List<ChatMessageViewModel> GetMessages()
        {
            var messages = _communityChatMessageRepository.GetAllMessagesWithUsers().Select(x => new ChatMessageViewModel()
            {
                Message = x.MessageText,
                TimeStamp = x.CreatedAt,
                UserName = x.CreatedByUser.Name,
                UserId = x.CreatedByUser.Id,
                IsOwnMessage = _authService.GetUserId() == x.CreatedByUser.Id
            }).ToList();

            return messages;
        }
    }
}

using DelightBistroMvc.Data.Models;
using DelightBistroMvc.Data.Repositories.Interfaces;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DTOs.Chat;
using DelightBistroMvc.Services.Chat.Interfaces;
using System.Security.Claims;

namespace DelightBistroMvc.Services.Chat
{
    public class NewChatService : INewChatService
    {
        private const int MAX_TEXT_LENGTH = 500;
        private readonly IChatMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NewChatService(IChatMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ChatMessageDto>> GetRecentMessageAsync(
            int count = 10,
            CancellationToken cancellationToken = default)
        {
            var list = await _messageRepository.GetRecentAsync(count, cancellationToken);
            return list.Select(MapToDto).ToList();
        }

        public string ResolveDisplayName(ClaimsPrincipal? user, string connectionId)
        {
            var nameFromCoockie = user?.FindFirstValue(AuthService.COOCKIE_NAME_KEY);
            if (string.IsNullOrEmpty(nameFromCoockie))
            {
                var suffix = connectionId.Length >= 4 ? connectionId[^4..] : connectionId;
                return $"Anonimus-{suffix}";
            }

            return nameFromCoockie;
        }

        public async Task<ChatMessageDto?> SaveMessageAsync(string senderName,
            string text,
            int? userId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var textTrim = text.Trim();
            if (textTrim.Length > MAX_TEXT_LENGTH)
            {
                textTrim = textTrim[..MAX_TEXT_LENGTH];
            }

            var messageData = new ChatMessageData
            {
                SenderName = senderName,
                Text = textTrim,
                CreatedAtUtc = DateTime.UtcNow,
                UserId = userId
            };
            await _messageRepository.AddAsync(messageData, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToDto(messageData);
        }

        public int? TryGetUserId(ClaimsPrincipal? user)
        {
            var idStr = user?.FindFirstValue(AuthService.COOCKIE_ID_KEY);
            if (string.IsNullOrEmpty(idStr) || !int.TryParse(idStr, out var userId) || userId <= 0)
            {
                return null;
            }
            return userId;
        }

        private ChatMessageDto MapToDto(ChatMessageData chatMessageData)
        {
            return new ChatMessageDto(chatMessageData.Id,
                chatMessageData.SenderName,
                chatMessageData.Text,
                chatMessageData.CreatedAtUtc);
        }
    }
}

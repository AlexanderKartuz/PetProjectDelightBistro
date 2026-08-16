using DelightBistroMvc.Models.DTOs.Chat;
using System.Collections.Concurrent;

namespace DelightBistroMvc.Services.Chat
{
    public class ChatPresenceService
    {
        private readonly ConcurrentDictionary<string, string> _users = new();

        public void Join(string connectionId, string userNmae)
        {
            _users[connectionId] = userNmae;
        }

        public bool Leave(string connectionId, out string? userName)
        {
            return _users.TryRemove(connectionId, out userName);
        }

        public List<ChatUserDto> GetOthers(string excludeConnectionId)
        {
            return _users.Where(u => u.Key != excludeConnectionId)
                .Select(u => new ChatUserDto(u.Key, u.Value))
                .ToList();
        }

        public string? GetUserName(string connectionId)
        {
            return _users.TryGetValue(connectionId, out var userName) ? userName : null;
        }
    }
}

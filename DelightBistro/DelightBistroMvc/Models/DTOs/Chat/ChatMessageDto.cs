namespace DelightBistroMvc.Models.DTOs.Chat
{
    public record ChatMessageDto(int Id, string SenderName, string Text, DateTime CreatedAtUtc);

}

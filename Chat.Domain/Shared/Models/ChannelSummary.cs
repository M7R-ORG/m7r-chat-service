namespace Chat.Domain.Shared.Models;

public class ChannelSummary
{
    public int ChannelId { get; set; }
    public int UnreadMessagesCount { get; set; }
    public string? LastMessageText { get; set; }
    public string? LastMessageAuthor { get; set; }
    public int LastMessageAttachmentsCount { get; set; }
}

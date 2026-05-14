namespace Chat.Application.Services.ChannelService.Models;

public class ChannelServiceLastMessageForOneResponseData
{
    public string? Content { get; set; }
    public string? Author { get; set; }
}

public class ChannelServiceAccountChannelResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public DateTime LastActivity { get; set; }
    public string? ImageId { get; set; }
    public string? UserActivityStatus { get; set; }
    public DateTime? UserLastOnlineAt { get; set; }
    public int? MembersCount { get; set; }
    public ChannelServiceLastMessageForOneResponseData? LastMessage { get; set; }
    public int UnreadMessagesCount { get; set; }
}

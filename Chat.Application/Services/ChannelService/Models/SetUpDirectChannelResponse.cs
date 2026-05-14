namespace Chat.Application.Services.ChannelService.Models;

public class ChannelServiceDirectChannel
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
}

public class ChannelServiceSetUpDirectChannelResponse
{
    public ChannelServiceDirectChannel? DirectChannel { get; set; }
    public IEnumerable<string> UserIds { get; set; } = [];
    public bool IsNeedNotifyUsers { get; set; } = false;
}

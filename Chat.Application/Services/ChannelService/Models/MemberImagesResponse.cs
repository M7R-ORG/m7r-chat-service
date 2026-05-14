namespace Chat.Application.Services.ChannelService.Models;

public class ChannelServiceMemberImageResponseData
{
    public int Id { get; set; }
    public string? ImageId { get; set; }
}

public class ChannelServiceMemberImagesResponse
{
    public required IEnumerable<ChannelServiceMemberImageResponseData> MemberImages { get; set; }
}

namespace Chat.WebApi.Controllers.Models.Chat;

public class ChatHubUploadFileRequest
{
    public required string UniqueId { get; set; }
    public required string FileId { get; set; }
    public required string Type { get; set; }
    public required int Size { get; set; }
    public required string Name { get; set; }
    public required int ChannelId { get; set; }
}

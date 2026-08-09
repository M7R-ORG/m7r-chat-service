using Chat.Domain.Shared.Models;

namespace Chat.Infrastructure.Services.AIService.Models;

public class AIIServiceCreateMessageRequest
{
    public int? OriginalMessageId { get; set; }
    public required int AccountId { get; set; }
    public required int ProfileId { get; set; }
    public required int ChannelId { get; set; }
    public required IEnumerable<AIMessage> Messages { get; set; } = [];
}

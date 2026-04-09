using Chat.Domain.Shared.Models;

namespace Chat.Domain.Entities.Messages;

public interface IMessageRepository : IAsyncRepository<Message>
{
    Task<Dictionary<int, ChannelSummary>> GetChannelSummariesAsync(
        IEnumerable<int> channelIds,
        int accountId
    );

    Task<List<int>> BatchReadMessagesAsync(
        int channelId,
        int lastMessageId,
        int accountId
    );
}

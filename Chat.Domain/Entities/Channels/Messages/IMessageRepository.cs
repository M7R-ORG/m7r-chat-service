using Chat.Domain.Shared.Models;

namespace Chat.Domain.Entities.Messages;

public interface IMessageRepository : IAsyncRepository<Message>
{
    Task<Dictionary<int, ChannelSummary>> GetChannelSummariesAsync(
        IEnumerable<int> channelIds,
        int accountId
    );
}

using Chat.Domain.Entities.Messages;
using Chat.Domain.Shared.Models;
using Chat.Persistence.DBContext;
using Chat.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(EFContext dbContext)
        : base(dbContext) { }

    public async Task<Dictionary<int, ChannelSummary>> GetChannelSummariesAsync(
        IEnumerable<int> channelIds,
        int accountId
    )
    {
        var channelIdList = channelIds.ToList();

        return await _dbSet
            .Where(message => channelIdList.Contains(message.ChannelId))
            .GroupBy(message => message.ChannelId)
            .Select(group => new ChannelSummary
            {
                ChannelId = group.Key,
                UnreadMessagesCount = group.Count(message =>
                    message.AuthorId != accountId
                    && message.ReadAccounts.All(account => account.Id != accountId)),
                LastMessageText = group
                    .OrderByDescending(message => message.CreatedAt)
                    .Select(message => message.Text)
                    .FirstOrDefault(),
                LastMessageAuthor = group
                    .OrderByDescending(message => message.CreatedAt)
                    .Select(message => message.Author != null ? message.Author.Login : null)
                    .FirstOrDefault(),
                LastMessageAttachmentsCount = group
                    .OrderByDescending(message => message.CreatedAt)
                    .Select(message => message.Attachments.Count)
                    .FirstOrDefault()
            })
            .ToDictionaryAsync(summary => summary.ChannelId);
    }
}

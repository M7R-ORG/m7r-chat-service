using Chat.Domain.Entities.Messages;
using Chat.Domain.Shared.Models;
using Chat.Persistence.DBContext;
using Chat.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    private readonly EFContext _dbContext;

    public MessageRepository(EFContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<int>> BatchReadMessagesAsync(
        int channelId,
        int lastMessageId,
        int accountId
    )
    {
        List<int> unreadMessageIds = await _dbSet
            .Where(message =>
                message.ChannelId == channelId
                && message.Id <= lastMessageId
                && message.ReadAccounts.All(account => account.Id != accountId)
                && message.AuthorId != accountId)
            .Select(message => message.Id)
            .ToListAsync();

        if (unreadMessageIds.Count == 0)
            return unreadMessageIds;

        await _dbSet
            .Where(message => unreadMessageIds.Contains(message.Id))
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(m => m.IsRead, true));

        List<Dictionary<string, object>> joinEntries = [.. unreadMessageIds.Select(messageId => new Dictionary<string, object>
        {
            ["ReadMessagesId"] = messageId,
            ["ReadAccountsId"] = accountId
        })];

        _dbContext.Set<Dictionary<string, object>>("AccountMessage").AddRange(joinEntries);

        await _dbContext.SaveChangesAsync();

        return unreadMessageIds;
    }

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

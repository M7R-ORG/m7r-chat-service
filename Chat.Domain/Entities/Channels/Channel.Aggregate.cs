using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Messages;
using Chat.Domain.Shared.Constants.Common;

namespace Chat.Domain.Entities.Channels;

public partial class Channel : IAggregateRoot
{
    public void AddAccount(Account account)
    {
        Accounts.Add(account);
    }

    public void AddAccounts(List<Account> accounts)
    {
        accounts.ForEach(Accounts.Add);
    }

    public void AddMessage(Message message)
    {
        Messages.Add(message);
    }

    public void SetOwner(int ownerId)
    {
        OwnerId = ownerId;
    }

    public Message? GetLastMessage()
    {
        return Messages.OrderByDescending(message => message.CreatedAt).FirstOrDefault();
    }

    public int GetUnreadMessagesCount(int authorId)
    {
        return Messages.Count(
            message =>
                message.AuthorId != authorId
                && message.ReadAccounts.All(account => account.Id != authorId)
        );
    }

    public void UpdateLastActivity()
    {
        LastActivity = DateTime.UtcNow;
    }

    public void UpdateImage(string? image)
    {
        if (Type == ChannelType.Private || Type == ChannelType.Public)
        {
            Image = image;
        }
    }

    public void SetAIProfileId(int? aiProfileId)
    {
        AIProfileId = aiProfileId;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}

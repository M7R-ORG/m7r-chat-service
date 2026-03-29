using Chat.Domain.Entities.Channels;
using Chat.Domain.Entities.Messages;

namespace Chat.Domain.Entities.Accounts;

public partial class Account : IAggregateRoot
{
    public void UpdateLogin(string login)
    {
        Login = login;
    }

    public void UpdateActivityStatus(string activityStatus)
    {
        ActivityStatus = activityStatus;
        LastOnlineAt = DateTime.UtcNow;
    }

    public void UpdateImage(string? image)
    {
        Image = image;
    }

    public void UpdatePassword(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public void AddChannel(Channel channel)
    {
        Channels.Add(channel);
    }

    public void AddOwnedChannel(Channel channel)
    {
        OwnedChannels.Add(channel);
    }

    public void AddReadMessage(Message message)
    {
        ReadMessages.Add(message);
    }

    public void AddMessage(Message message)
    {
        Messages.Add(message);
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}

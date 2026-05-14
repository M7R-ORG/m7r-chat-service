using Chat.Application.Services.ChannelService.Models;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Channels;
using Chat.Domain.Entities.Messages;

public class ChannelServiceDirectChannelAdapter : ChannelServiceDirectChannel
{
    public ChannelServiceDirectChannelAdapter(Channel channel, int authorId)
    {
        Id = channel.Id;
        Type = channel.Type;
        LastActivity = channel.LastActivity;

        Message? lastMessage = channel.GetLastMessage();

        if (lastMessage != null)
        {
            LastMessage = new ChannelServiceLastMessageForOneResponseData()
            {
                Author = lastMessage.Author?.Login,
                Content = lastMessage.Text
            };
        }

        Account? chatPartner = channel.Accounts.FirstOrDefault(account => account.Id != authorId);

        if (chatPartner != null)
        {
            ImageId = chatPartner.Image;
            Name = channel.Name ?? chatPartner.Login;
            UserActivityStatus = chatPartner.ActivityStatus;
            UserLastOnlineAt = chatPartner.LastOnlineAt;
        }
    }
}

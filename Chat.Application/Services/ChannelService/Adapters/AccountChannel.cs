using Chat.Application.Services.ChannelService.Models;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Channels;
using Chat.Domain.Entities.Messages;
using Chat.Domain.Shared.Constants.Common;
using Chat.Domain.Shared.Models;

namespace Chat.Application.Services.ChatService.Adapters;

public class ChannelServiceAccountChannelListAdapter : ChannelServiceAccountChannelResponseData
{
    public ChannelServiceAccountChannelListAdapter(
        Channel channel,
        int authorId,
        ChannelSummary? summary
    )
    {
        Id = channel.Id;
        Type = channel.Type;
        LastActivity = channel.LastActivity;
        UnreadMessagesCount = summary?.UnreadMessagesCount ?? 0;

        if (summary is { LastMessageText: not null } or { LastMessageAttachmentsCount: > 0 })
        {
            string lastMessageContent = string.IsNullOrEmpty(summary!.LastMessageText)
                ? $"{summary.LastMessageAttachmentsCount} attachments"
                : summary.LastMessageText;

            LastMessage = new ChannelServiceLastMessageResponseData()
            {
                Author = summary.LastMessageAuthor,
                Content = lastMessageContent,
            };
        }

        if (Type == ChannelType.Direct)
        {
            Account? chatPartner = channel
                .Accounts
                .FirstOrDefault(account => account.Id != authorId);

            if (chatPartner != null)
            {
                ImageId = chatPartner.Image;
                Name = channel.Name ?? chatPartner.Login;
            }
        }
        else
        {
            ImageId = channel.Image;
            Name = channel.Name;
        }
    }
}

public class ChannelServiceAccountChannelAdapter : ChannelServiceAccountChannelResponse
{
    public ChannelServiceAccountChannelAdapter(Channel channel, int authorId)
    {
        Id = channel.Id;
        Type = channel.Type;
        LastActivity = channel.LastActivity;
        UnreadMessagesCount = channel.GetUnreadMessagesCount(authorId);

        Message? lastMessage = channel.GetLastMessage();

        if (lastMessage != null)
        {
            int attachmentsCount = lastMessage.Attachments.Count;
            string lastMessageContent = string.IsNullOrEmpty(lastMessage.Text)
                ? $"{attachmentsCount} attachments"
                : lastMessage.Text;

            LastMessage = new ChannelServiceLastMessageForOneResponseData()
            {
                Author = lastMessage.Author?.Login,
                Content = lastMessageContent
            };
        }

        if (Type == ChannelType.Direct)
        {
            Account? chatPartner = channel
                .Accounts
                .FirstOrDefault(account => account.Id != authorId);

            if (chatPartner != null)
            {
                ImageId = chatPartner.Image;
                Name = channel.Name ?? chatPartner.Login;
                UserActivityStatus = chatPartner.ActivityStatus;
                UserLastOnlineAt = chatPartner.LastOnlineAt;
            }
        }
        else
        {
            ImageId = channel.Image;
            Name = channel.Name;
            MembersCount = channel.Accounts.Count;
        }
    }
}

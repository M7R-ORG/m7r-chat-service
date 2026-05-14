using Chat.Application.Services.ChannelService.Models;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Channels;
using Chat.Domain.Shared.Constants.Common;

namespace Chat.Application.Services.ChatService.Adapters;

public class ChannelServicePublicChannelAdapter : ChannelServicePublicChannelResponseData
{
    public ChannelServicePublicChannelAdapter(Channel channel, int authorId)
    {
        Id = channel.Id;
        Type = channel.Type;
        LastActivity = channel.LastActivity;

        if (Type == ChannelType.Direct)
        {
            Account? chatPartner = channel
                .Accounts
                .FirstOrDefault(account => account.Id != authorId);

            if (chatPartner != null)
            {
                ImageId = chatPartner.Image;
                Name = chatPartner.Login;
            }
        }
        else
        {
            ImageId = channel.Image;
            Name = channel.Name;
        }
    }
}

using Chat.Application.Services.ChannelService.Models;
using Chat.Application.Services.ChatService.Adapters;
using Chat.Application.Services.Common;
using Chat.Domain.Common;
using Chat.Domain.Entities.Channels;
using Chat.Domain.Exceptions;
using Chat.Domain.Services.ChannelService;
using Chat.Domain.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Services.ChannelService;

public class ChannelService : BaseService, IChannelService
{
    private readonly ChannelBS _channelBS;

    public ChannelService(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor context,
        IAppSettings appSettings,
        ChannelBS channelBS
    )
        : base(unitOfWork, context, appSettings)
    {
        _channelBS = channelBS;
    }

    public async Task<ChannelServiceCreateDirectChannelResponse> CreateDirectChannelAsync(
        ChannelServiceCreateDirectChannelRequest request
    )
    {
        Channel channel = await _channelBS.CreateDirectChannelAsync(
            AccountId,
            request.AccountId,
            request.Name,
            request.AIProfileId
        );

        return new ChannelServiceCreateDirectChannelResponse()
        {
            IsSuccess = true,
            ChannelId = channel.Id,
        };
    }

    public async Task<ChannelServiceCreatePrivateChannelResponse> CreatePrivateChannelAsync(
        ChannelServiceCreatePrivateChannelRequest request
    )
    {
        await _channelBS.CreatePrivateChannelAsync(
            AccountId,
            request.Name,
            request.Members,
            request.AIProfileId
        );

        return new ChannelServiceCreatePrivateChannelResponse() { IsSuccess = true };
    }

    public async Task<ChannelServiceCreatePublicChannelResponse> CreatePublicChannelAsync(
        ChannelServiceCreatePublicChannelRequest request
    )
    {
        await _channelBS.CreatePublicChannelAsync(
            AccountId,
            request.Name,
            request.Members,
            request.AIProfileId
        );

        return new ChannelServiceCreatePublicChannelResponse() { IsSuccess = true };
    }

    public async Task<ChannelServiceConnectToChannelResponse> ConnectToChannelAsync(
        ChannelServiceConnectToChannelRequest request
    )
    {
        await _channelBS.ConnectToChannelAsync(AccountId, request.ChannelId);

        return new ChannelServiceConnectToChannelResponse() { IsSuccess = true };
    }

    public async Task<ChannelServicePublicChannelsResponse> PublicChannelsAsync(
        ChannelServicePublicChannelsRequest request
    )
    {
        PaginatorResponse<Channel> paginatedData = await _channelBS.PublicChannelsPaginatedAsync(
            request.SearchField,
            request.Pagination
        );

        var adaptedChannels = paginatedData
            .Collection
            .Select(channel => new ChannelServicePublicChannelAdapter(channel, AccountId))
            .ToList();

        return new ChannelServicePublicChannelsResponse()
        {
            Meta = paginatedData.Meta,
            Channels = adaptedChannels
        };
    }

    public async Task<ChannelServiceAccountChannelsResponse> AccountChannelsAsync(
        ChannelServiceAccountChannelsRequest request
    )
    {
        PaginatorResponse<Channel> paginatedData = await _channelBS.AccountChannelsPaginatedAsync(
            AccountId,
            request.SearchField,
            request.ChannelType,
            request.Pagination
        );

        IEnumerable<int> channelIds = paginatedData.Collection.Select(channel => channel.Id);
        Dictionary<int, ChannelSummary> summaries = await _unitOfWork.Message.GetChannelSummariesAsync(channelIds, AccountId);

        var adaptedChannels = paginatedData
            .Collection
            .Select(channel => new ChannelServiceAccountChannelListAdapter(
                channel,
                AccountId,
                summaries.GetValueOrDefault(channel.Id)
            ))
            .ToList();

        return new ChannelServiceAccountChannelsResponse()
        {
            Meta = paginatedData.Meta,
            Channels = adaptedChannels
        };
    }

    public async Task<ChannelServiceAccountChannelResponse> AccountChannelAsync(
        ChannelServiceAccountChannelRequest request
    )
    {
        Channel channel = await _channelBS.AccountChannelAsync(AccountId, request.Id);

        var adaptedChannel = new ChannelServiceAccountChannelAdapter(channel, AccountId);

        return adaptedChannel;
    }

    public async Task<ChannelServiceSetUpDirectChannelResponse> SetUpDirectChannelAsync(
        ChannelServiceSetUpDirectChannelRequest request
    )
    {
        Channel? channel = await _channelBS.AccountDirectChannelAsync(AccountId, request.PartnerId);

        Channel? directChannel;
        bool isNeedNotify = false;

        if (channel != null)
        {
            directChannel = channel;
        }
        else
        {
            Channel newChannel = await _channelBS.CreateDirectChannelAsync(
                AccountId,
                request.PartnerId
            );

            directChannel = await _channelBS.AccountDirectChannelAsync(
                AccountId,
                request.PartnerId
            );
            isNeedNotify = true;
        }

        if (directChannel == null)
            throw new NotExistsException("Channel not exists");

        IEnumerable<string> userIds = directChannel
            .Accounts
            .Select(account => account.Id.ToString());

        var adaptedChannel = new ChannelServiceDirectChannelAdapter(directChannel, AccountId);

        return new ChannelServiceSetUpDirectChannelResponse()
        {
            DirectChannel = adaptedChannel,
            UserIds = userIds,
            IsNeedNotifyUsers = isNeedNotify
        };
    }

    public async Task<ChannelServiceMemberImagesResponse> MemberImagesAsync(
        ChannelServiceMemberImagesRequest request
    )
    {
        Channel channel = await _channelBS.AccountChannelAsync(AccountId, request.ChannelId);

        IEnumerable<ChannelServiceMemberImageResponseData> memberImages = channel
            .Accounts
            .Select(account => new ChannelServiceMemberImageResponseData()
            {
                Id = account.Id,
                ImageId = account.Image
            });

        return new ChannelServiceMemberImagesResponse() { MemberImages = memberImages };
    }
}

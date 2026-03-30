using Chat.Domain.Common;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Accounts.AIBots;
using Chat.Domain.Entities.Channels;
using Chat.Domain.Exceptions;
using Chat.Domain.Shared.Constants.Common;
using Chat.Domain.Shared.Models;
using Chat.Domain.Specification;

namespace Chat.Domain.Services.ChannelService;

public class ChannelBS : DomainService
{
    public ChannelBS(IAppSettings appSettings, IUnitOfWork unitOfWork)
        : base(appSettings, unitOfWork) { }

    public async Task<Channel> CreateDirectChannelAsync(
        int firstAccountId,
        int secondAccountId,
        string? name = null,
        int? aiProfileId = null
    )
    {
        ISingleSpecification<Account> specification =
            aiProfileId == null
                ? new AccountByIdSpec(secondAccountId, true)
                : new FirstAIBotAccountSpec();

        Account? firstAccount = await _unitOfWork
            .Account
            .GetAsync(new AccountByIdSpec(firstAccountId, true));

        Account? secondAccount = await _unitOfWork.Account.GetAsync(specification);

        if (firstAccount == null || secondAccount == null)
            throw new NotExistsException("Account not found");

        bool isExistsSameDirectChannel = await _unitOfWork
            .Channel
            .AnyAsync(
                (channel) =>
                    channel.Type == ChannelType.Direct
                    && channel.Accounts.Contains(firstAccount)
                    && channel.Accounts.Contains(secondAccount)
                    && !(secondAccount is AIBot)
            );

        if (isExistsSameDirectChannel)
            throw new AlreadyExistsException("This channel already exists");

        var channel = new Channel(ChannelType.Direct) { Name = name };

        channel.SetAIProfileId(aiProfileId);

        List<Account> memberList = [firstAccount, secondAccount];

        if (aiProfileId != null)
        {
            AIBot aiBotAccount =
                await _unitOfWork.AIBot.GetAsync(new FirstAIBotSpec())
                ?? throw new NotExistsException("AIBot not found");

            memberList.Add(aiBotAccount);
        }

        channel.AddAccounts(memberList);

        await _unitOfWork.Channel.AddAsync(channel);
        await _unitOfWork.SaveChangesAsync();

        return channel;
    }

    public async Task<Channel> CreatePrivateChannelAsync(
        int accountId,
        string channelName,
        IEnumerable<int> members,
        int? aiProfileId = null
    )
    {
        Account myAccount =
            await _unitOfWork.Account.GetAsync(new AccountByIdSpec(accountId, true))
            ?? throw new NotExistsException("Account not found");

        bool isExistsSameChannel = await _unitOfWork
            .Channel
            .AnyAsync(channel => channel.Name == channelName && channel.OwnerId == accountId);

        if (isExistsSameChannel)
            throw new AlreadyExistsException("This channel name already exists");

        var channel = new Channel(ChannelType.Private) { Name = channelName };

        channel.SetAIProfileId(aiProfileId);
        channel.SetOwner(accountId);
        channel.AddAccount(myAccount);

        IEnumerable<Account> accounts = await _unitOfWork
            .Account
            .GetAllAsync(new AccountsByIdsSpec(members));

        var accountList = accounts.ToList();

        if (aiProfileId != null)
        {
            AIBot aiBotAccount =
                await _unitOfWork.AIBot.GetAsync(new FirstAIBotSpec())
                ?? throw new NotExistsException("AIBot not found");

            accountList.Add(aiBotAccount);
        }

        channel.AddAccounts(accountList);

        await _unitOfWork.Channel.AddAsync(channel);
        await _unitOfWork.SaveChangesAsync();

        return channel;
    }

    public async Task<Channel> CreatePublicChannelAsync(
        int accountId,
        string channelName,
        IEnumerable<int> members,
        int? aiProfileId = null
    )
    {
        Account myAccount =
            await _unitOfWork.Account.GetAsync(new AccountByIdSpec(accountId, true))
            ?? throw new NotExistsException("Account not found");

        bool isExistsSameChannel = await _unitOfWork
            .Channel
            .AnyAsync(channel => channel.Name == channelName && channel.OwnerId == accountId);

        if (isExistsSameChannel)
            throw new AlreadyExistsException("This channel name already exists");

        var channel = new Channel(ChannelType.Public) { Name = channelName };

        channel.SetAIProfileId(aiProfileId);
        channel.SetOwner(accountId);
        channel.AddAccount(myAccount);

        IEnumerable<Account> accounts = await _unitOfWork
            .Account
            .GetAllAsync(new AccountsByIdsSpec(members));

        var accountList = accounts.ToList();

        if (aiProfileId != null)
        {
            AIBot aiBotAccount =
                await _unitOfWork.AIBot.GetAsync(new FirstAIBotSpec())
                ?? throw new NotExistsException("AIBot not found");

            accountList.Add(aiBotAccount);
        }

        channel.AddAccounts(accountList);

        await _unitOfWork.Channel.AddAsync(channel);
        await _unitOfWork.SaveChangesAsync();

        return channel;
    }

    public async Task ConnectToChannelAsync(int accountId, int channelId)
    {
        Account account =
            await _unitOfWork.Account.GetAsync(new AccountByIdSpec(accountId, true))
            ?? throw new NotExistsException("Account not found");

        Channel channel =
            await _unitOfWork.Channel.GetAsync(new ChannelByIdSpec(channelId))
            ?? throw new NotExistsException("Channel not found");

        if (channel.Type != ChannelType.Public || channel.Accounts.Contains(account))
            throw new IncorrectDataException("Invalid channel");

        channel.AddAccount(account);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Channel>> PublicChannelsAsync(string? searchField)
    {
        IEnumerable<Channel>? channels = await _unitOfWork
            .Channel
            .GetAllAsync(new PublicChannelsSpec(searchField));

        if (channels == null)
            throw new NotExistsException("Channels not exists");

        return channels;
    }

    public async Task<PaginatorResponse<Channel>> PublicChannelsPaginatedAsync(
        string? searchField,
        Pagination? pagination
    )
    {
        return await _unitOfWork.Channel.GetPaginatedAsync(
            new PublicChannelsSpec(searchField),
            pagination
        );
    }

    public async Task<IEnumerable<Channel>> AccountChannelsAsync(
        int accountId,
        string? searchField,
        string? channelType
    )
    {
        IEnumerable<Channel>? channels = await _unitOfWork
            .Channel
            .GetAllAsync(new AccountChannelsSpec(accountId, searchField, channelType));

        if (channels == null)
            throw new NotExistsException("Channels not exists");

        return channels;
    }

    public async Task<PaginatorResponse<Channel>> AccountChannelsPaginatedAsync(
        int accountId,
        string? searchField,
        string? channelType,
        Pagination? pagination
    )
    {
        return await _unitOfWork.Channel.GetPaginatedAsync(
            new AccountChannelsSpec(accountId, searchField, channelType),
            pagination
        );
    }

    public async Task<Channel> AccountChannelAsync(int accountId, int channelId)
    {
        Channel? channel = await _unitOfWork
            .Channel
            .GetAsync(new AccountChannelSpec(accountId, channelId));

        if (channel == null)
            throw new NotExistsException("Channel not exists");

        return channel;
    }

    public async Task<Channel?> AccountDirectChannelAsync(int accountId, int partnerId)
    {
        Channel? channel = await _unitOfWork
            .Channel
            .GetAsync(new AccountDirectChannelSpec(accountId, partnerId));

        return channel;
    }
}

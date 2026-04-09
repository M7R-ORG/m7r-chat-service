using Chat.Application.Services.ChatService.Adapters;
using Chat.Application.Services.ChatService.Extensions;
using Chat.Application.Services.ChatService.Models;
using Chat.Application.Services.Common;
using Chat.Domain.Common;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Attachments;
using Chat.Domain.Entities.Messages;
using Chat.Domain.Exceptions;
using Chat.Domain.Services;
using Chat.Domain.Services.ChatService;
using Chat.Domain.Shared.Constants.Common;
using Chat.Domain.Shared.Models;
using Chat.Infrastructure.Services.AIService;
using Chat.Infrastructure.Services.AIService.Models;
using Chat.Persistence.Extensions;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Services.ChatService;

public class ChatService : BaseService, IChatService
{
    private readonly ChatBS _chatBS;
    private readonly AttachmentBS _attachmentBS;
    private readonly IAIIS _aiIS;

    public ChatService(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor context,
        IAppSettings appSettings,
        ChatBS chatBS,
        AttachmentBS attachmentBS,
        IAIIS aiIS
    )
        : base(unitOfWork, context, appSettings)
    {
        _chatBS = chatBS;
        _attachmentBS = attachmentBS;
        _aiIS = aiIS;
    }

    public async Task<ChatServiceMessagesResponse> MessagesAsync(ChatServiceMessagesRequest request)
    {
        PaginatorResponse<Message> paginatedData = await _chatBS.MessagesPaginatedAsync(
            request.ChannelId,
            request.SearchField,
            request.Pagination
        );

        IEnumerable<ChatServiceMessageAdapter> adaptedMessages = paginatedData
            .Collection
            .Select(message => new ChatServiceMessageAdapter(message));

        return new ChatServiceMessagesResponse()
        {
            Meta = paginatedData.Meta,
            Messages = adaptedMessages
        };
    }

    public async Task<ChatServiceSendMessageResponse> SendMessageAsync(
        ChatServiceSendMessageRequest request
    )
    {
        Message message = await _chatBS.AddMessageAsync(
            AccountId,
            request.ChannelId,
            request.Message,
            request.Attachments
        );

        int? aiProfileId = await _chatBS.GetAIProfileIdByChannelIdAsync(request.ChannelId);

        if (aiProfileId != null && Role != AccountRole.AIBot)
        {
            IEnumerable<Message> messages = await _chatBS.MessagesForAIAsync(request.ChannelId);

            _aiIS.CreateMessage(
                new AIIServiceCreateMessageRequest()
                {
                    OriginalMessageId = message.Id,
                    ChannelId = request.ChannelId,
                    ProfileId = aiProfileId.Value,
                    Messages = messages.AdaptForAI()
                }
            );
        }

        IEnumerable<string> userIds = await _chatBS.GetUserIdsByChannelIdAsync(
            AccountId,
            request.ChannelId
        );

        return new ChatServiceSendMessageResponse()
        {
            UserIds = userIds,
            Message = new ChatServiceMessageAdapter(message)
        };
    }

    public async Task<ChatServiceCreateAIMessageResponse> CreateAIMessageAsync(
        ChatServiceCreateAIMessageRequest request
    )
    {
        int aiBotId = await _chatBS.GetAIBotId();

        Message message = await _chatBS.AddMessageAsync(
            aiBotId,
            request.ChannelId,
            request.Message,
            [],
            request.OriginalMessageId
        );

        IEnumerable<string> userIds = await _chatBS.GetUserIdsByChannelIdAsync(
            aiBotId,
            request.ChannelId
        );

        return new ChatServiceCreateAIMessageResponse()
        {
            UserIds = userIds,
            Message = new ChatServiceMessageAdapter(message)
        };
    }

    public async Task<ChatServiceReadMessageResponse> ReadMessageAsync(
        ChatServiceReadMessageRequest request
    )
    {
        int accountId = request.IsAIBot ? await _chatBS.GetAIBotId() : AccountId;

        List<int> readMessageIds = await _chatBS.ReadMessagesAsync(
            request.ChannelId,
            request.MessageId,
            accountId
        );

        IEnumerable<string> userIds = await _chatBS.GetUserIdsByChannelIdAsync(
            accountId,
            request.ChannelId
        );

        int unreadMessagesCount = await _chatBS.GetUnreadMessagesCountAsync(
            accountId,
            request.ChannelId
        );

        return new ChatServiceReadMessageResponse()
        {
            ReadMessageIds = readMessageIds,
            UserIds = userIds,
            UnreadMessagesCount = unreadMessagesCount
        };
    }

    public async Task<ChatServiceUploadAttachmentResponse> UploadAttachmentAsync(
        ChatServiceUploadAttachmentRequest request
    )
    {
        Attachment attachment = await ProcessAttachment.CreateFileAsync(request, _appSettings);

        attachment.SetChannel(request.ChannelId);
        attachment.SetOwner(AccountId);

        await _attachmentBS.CreateAttachmentAsync(attachment);

        return new ChatServiceUploadAttachmentResponse() { AttachmentId = attachment.Id };
    }

    public async Task<ChatServiceLoadAttachmentResponse> LoadAttachmentAsync(
        ChatServiceLoadAttachmentRequest request
    )
    {
        Attachment? attachment =
            await _attachmentBS.GetAttachmentByIdAsync(request.AttachmentId)
            ?? throw new NotExistsException("Attachment not found");

        ICollection<Account>? accounts = attachment.Message?.Channel?.Accounts ?? [];

        if (attachment.OwnerId != AccountId && !accounts.Any(account => account.Id == AccountId))
            throw new OperationNotAllowedException();

        byte[] contentBytes = await FileManager.ReadToBytesAsync(attachment.Content) ?? [];

        return new ChatServiceLoadAttachmentResponse()
        {
            Id = attachment.Id,
            Content = contentBytes,
            Type = attachment.Type,
            Name = attachment.Name,
            Size = attachment.Size
        };
    }

    public async Task<ChatServiceRemoveAttachmentResponse> RemoveAttachmentAsync(
        ChatServiceRemoveAttachmentRequest request
    )
    {
        Attachment? attachment =
            await _attachmentBS.GetAttachmentByUniqueIdAsync(request.UniqueId)
            ?? throw new NotExistsException("Attachment not found");

        ProcessAttachment.RemoveFile(attachment);
        await _attachmentBS.RemoveAttachmentAsync(attachment);

        return new ChatServiceRemoveAttachmentResponse() { AttachmentId = attachment.Id };
    }

    public async Task<ChatServicePreviewAttachmentsResponse> PreviewAttachmentsAsync(
        ChatServicePreviewAttachmentsRequest request
    )
    {
        IEnumerable<Attachment> previewAttachments = await _attachmentBS.GetPreviewAttachmentsAsync(
            request.ChannelId,
            AccountId
        );

        return new ChatServicePreviewAttachmentsResponse()
        {
            PreviewAttachments = previewAttachments.Select(
                attachment => new ChatServicePreviewAttachmentAdapter(attachment)
            )
        };
    }
}

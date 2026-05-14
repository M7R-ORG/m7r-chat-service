using Chat.Application.Services.ChatService.Models;
using Chat.Domain.Entities.Attachments;

namespace Chat.Application.Services.ChatService.Adapters;

public class ChatServicePreviewAttachmentAdapter : PreviewAttachmentResponse
{
    public ChatServicePreviewAttachmentAdapter(Attachment attachment)
    {
        Id = attachment.Id;
        FileId = attachment.FileId;
        Type = attachment.Type;
        UniqueId = attachment.UniqueId;
        Name = attachment.Name;
        Size = attachment.Size;
    }
}

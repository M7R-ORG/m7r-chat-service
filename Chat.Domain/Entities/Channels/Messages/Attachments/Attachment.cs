using System.ComponentModel.DataAnnotations.Schema;
using Chat.Domain.Entities.Messages;

namespace Chat.Domain.Entities.Attachments;

[Table("Attachments")]
public partial class Attachment : BaseEntity, ISoftDelete
{
    public Attachment(string fileId, string type, string name, int size, string uniqueId)
    {
        FileId = fileId;
        Type = type;
        Name = name;
        Size = size;
        UniqueId = uniqueId;
    }

    public string FileId { get; private set; }
    public string Type { get; private set; }
    public string UniqueId { get; private set; }
    public string Name { get; private set; }
    public int Size { get; private set; }
    public bool IsDeleted { get; private set; } = false;
    public Message? Message { get; private set; }
    public int? MessageId { get; private set; }
    public int? ChannelId { get; private set; }
    public int? OwnerId { get; private set; }
}

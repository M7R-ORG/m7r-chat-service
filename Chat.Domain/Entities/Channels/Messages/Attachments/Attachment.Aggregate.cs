namespace Chat.Domain.Entities.Attachments;

public partial class Attachment : IAggregateRoot
{
    public void Delete()
    {
        IsDeleted = true;
    }

    public void SetMessage(int messageId)
    {
        MessageId = messageId;
    }
}

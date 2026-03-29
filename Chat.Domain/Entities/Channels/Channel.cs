using System.ComponentModel.DataAnnotations.Schema;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Messages;

namespace Chat.Domain.Entities.Channels;

[Table("Channels")]
public partial class Channel : BaseEntity, ISoftDelete
{
    public Channel(string type)
    {
        Type = type;
    }

    public string? Name { get; set; }
    public string Type { get; private set; }
    public DateTime LastActivity { get; private set; } = DateTime.UtcNow;
    public ICollection<Account> Accounts { get; private set; } = [];
    public ICollection<Message> Messages { get; private set; } = [];
    public Account? Owner { get; private set; }
    public int? OwnerId { get; private set; }
    public string? Image { get; set; }
    public bool IsDeleted { get; private set; } = false;
    public int? AIProfileId { get; private set; }
}

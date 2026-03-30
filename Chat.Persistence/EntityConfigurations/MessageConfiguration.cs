using Chat.Domain.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Persistence.EntityConfigurations;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasIndex(message => message.IsDeleted);
        builder.HasIndex(message => message.IsRead);
        builder.HasIndex(message => message.ChannelId);
        builder.HasIndex(message => message.AuthorId);
        builder.HasIndex(message => new { message.ChannelId, message.CreatedAt })
            .IsDescending(false, true);
    }
}

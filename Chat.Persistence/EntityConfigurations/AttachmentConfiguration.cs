using Chat.Domain.Entities.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Persistence.EntityConfigurations;

internal class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasIndex(attachment => attachment.UniqueId).IsUnique();
        builder.Property(attachment => attachment.UniqueId).IsRequired();
        builder.Property(attachment => attachment.FileId).IsRequired();
        builder.HasIndex(attachment => attachment.Type);
        builder.Property(attachment => attachment.Type).IsRequired();
        builder.Property(attachment => attachment.Name).IsRequired();
        builder.Property(attachment => attachment.Size).IsRequired();
        builder.HasIndex(attachment => attachment.OwnerId);
        builder.HasIndex(attachment => attachment.ChannelId);
        builder.Property(attachment => attachment.ChannelId).IsRequired();
        builder.HasIndex(attachment => attachment.IsDeleted);
    }
}

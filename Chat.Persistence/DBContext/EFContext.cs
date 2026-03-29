using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Accounts.Admins;
using Chat.Domain.Entities.Accounts.AIBots;
using Chat.Domain.Entities.Accounts.Users;
using Chat.Domain.Entities.Attachments;
using Chat.Domain.Entities.Channels;
using Chat.Domain.Entities.Messages;
using Chat.Persistence.EntityConfigurations;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.DBContext;

public class EFContext : DbContext, IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<AIBot> AIBots { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    public EFContext(DbContextOptions<EFContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        modelBuilder.ApplyConfiguration(new AIBotConfiguration());
        modelBuilder.ApplyConfiguration(new ChannelConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new AttachmentConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}

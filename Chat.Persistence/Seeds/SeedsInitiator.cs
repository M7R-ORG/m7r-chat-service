using Chat.Domain.Common;
using Chat.Persistence.DBContext;
using Chat.Persistence.Seeds.DefaultUsers;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Chat.Persistence.Seeds;

public static class SeedsInitiator
{
    public static void Apply(
        EFContext eFContext,
        IAppSettings appSettings,
        ILogger<EFContext> logger
    )
    {
        string password = appSettings.Seed.Password;

        using IDbContextTransaction transaction = eFContext.Database.BeginTransaction();
        try
        {
            DefaultUsersSeed.CreateAdmins(eFContext, password);
            DefaultUsersSeed.CreateUsers(eFContext, password);
            DefaultUsersSeed.CreateAIBots(eFContext, password);

            transaction.Commit();
            logger.LogInformation("Seeds: Success");
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            logger.LogDebug(exception, "Seeds: Error");
        }
    }
}

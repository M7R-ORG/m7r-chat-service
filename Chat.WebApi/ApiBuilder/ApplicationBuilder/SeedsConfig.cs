using Chat.Domain.Common;
using Chat.Persistence.DBContext;
using Chat.Persistence.Seeds;

namespace Chat.WebApi.ApiBuilder.ApplicationBuilder;

public static partial class ApplicationBuilderExtension
{
    public static void SeedsConfiguration(this WebApplication webApplication)
    {
        using IServiceScope scope = webApplication.Services.CreateScope();

        EFContext dbContext = scope.ServiceProvider.GetRequiredService<EFContext>();
        ILogger<EFContext> logger = scope.ServiceProvider.GetRequiredService<ILogger<EFContext>>();
        IAppSettings appSettings = scope.ServiceProvider.GetRequiredService<IAppSettings>();

        SeedsInitiator.Apply(dbContext, appSettings, logger);
    }
}

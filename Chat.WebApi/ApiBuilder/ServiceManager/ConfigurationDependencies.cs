using Chat.Domain.Shared.Settings;

namespace Chat.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddConfigurationDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration config
    )
    {
        serviceCollection.Configure<CommonSettings>(config.GetSection(CommonSettings.Path));
        serviceCollection.Configure<AuthSettings>(config.GetSection(AuthSettings.Path));
        serviceCollection.Configure<ClientSettings>(config.GetSection(ClientSettings.Path));
        serviceCollection.Configure<RoutePathSettings>(config.GetSection(RoutePathSettings.Path));
        serviceCollection.Configure<FilePathSettings>(config.GetSection(FilePathSettings.Path));
        serviceCollection.Configure<RMQSettings>(config.GetSection(RMQSettings.Path));
        serviceCollection.Configure<SeedSettings>(config.GetSection(SeedSettings.Path));

        return serviceCollection;
    }
}

using Chat.Domain.Common;
using Chat.Domain.Exceptions;
using Chat.Domain.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Chat.Infrastructure.AppSettings;

public class AppSettings(
    IOptions<CommonSettings> commonSettings,
    IOptions<RoutePathSettings> routePathSettings,
    IOptions<FilePathSettings> filePathSettings,
    IOptions<ClientSettings> clientSettings,
    IOptions<AuthSettings> authSettings,
    IOptions<RMQSettings> rmqSettings,
    IOptions<SeedSettings> seedSettings
) : IAppSettings
{
    public CommonSettings Common { get; } = commonSettings.Value;
    public RoutePathSettings RoutePath { get; } = routePathSettings.Value;
    public FilePathSettings FilePath { get; } = filePathSettings.Value;
    public ClientSettings Client { get; } = clientSettings.Value;
    public AuthSettings Auth { get; } = authSettings.Value;
    public RMQSettings RMQ { get; } = rmqSettings.Value;
    public SeedSettings Seed { get; } = seedSettings.Value;

    public static TSection GetSection<TSection>(IConfiguration configuration)
        where TSection : ISettings
    {
        TSection? section = configuration.GetSection(TSection.Path).Get<TSection>();

        if (section == null)
            throw new IncorrectDataException($"Incorrect config ({TSection.Path})", true);

        return section;
    }
}

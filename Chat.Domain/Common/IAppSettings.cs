using Chat.Domain.Shared.Settings;

namespace Chat.Domain.Common;

public interface IAppSettings
{
    CommonSettings Common { get; }
    RoutePathSettings RoutePath { get; }
    FilePathSettings FilePath { get; }
    ClientSettings Client { get; }
    AuthSettings Auth { get; }
    RMQSettings RMQ { get; }
    SeedSettings Seed { get; }
}

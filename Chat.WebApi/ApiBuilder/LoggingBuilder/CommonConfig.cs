namespace Chat.WebApi.ApiBuilder.LoggingBuilder;

public static partial class LoggingBuilderExtension
{
    public static ILoggingBuilder AddCommonConfiguration(
        this ILoggingBuilder loggingBuilder,
        IConfiguration configuration
    )
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();

        return loggingBuilder;
    }
}

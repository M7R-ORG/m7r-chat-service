namespace Chat.WebApi.ApiBuilder.ApplicationBuilder;

public static partial class ApplicationBuilderExtension
{
    public static void HealthConfiguration(this WebApplication webApplication)
    {
        webApplication.MapHealthChecks("/health/live").AllowAnonymous();
        webApplication.MapHealthChecks("/health/ready").AllowAnonymous();
    }
}

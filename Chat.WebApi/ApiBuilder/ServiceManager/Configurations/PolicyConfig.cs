using Chat.Domain.Shared.Constants.Common;
using Chat.Domain.Shared.Settings;
using Chat.Infrastructure.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Chat.WebApi.ApiBuilder.ServiceManager;

public static class PolicyConfigExtension
{
    private static readonly string[] s_allowOrigins = [];

    public static void PolicyConfig(this AuthorizationOptions authorizationOptions)
    {
        authorizationOptions.AddPolicy(AuthPolicy.OnlyUser, policy => policy.RequireRole("User"));
        authorizationOptions.AddPolicy(AuthPolicy.OnlyAdmin, policy => policy.RequireRole("Admin"));

        authorizationOptions.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    }

    public static void CorsConfig(this CorsOptions corsOptions, IConfiguration configuration)
    {
        ClientSettings clientSettings = AppSettings.GetSection<ClientSettings>(configuration);

        string[] origins = [.. s_allowOrigins, clientSettings.BaseUrl];

        corsOptions.AddPolicy(
            CorsPolicyName.Default,
            policy =>
                policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
        );
    }
}

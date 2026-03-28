using Chat.Application.Common;
using Chat.Persistence.DBContext;
using Chat.WebApi.Common;
using Chat.WebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddCommonDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration config
    )
    {
        string? dbConnection = AppEnvironment.GetDBConnectionString(config);
        string? redisConnection = AppEnvironment.GetRedisConnectionString(config);

        serviceCollection.AddOptions();
        serviceCollection.AddDbContext<EFContext>(options => options.UseNpgsql(dbConnection));
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddLogging();
        serviceCollection.AddCors(options => options.CorsConfig(config));
        serviceCollection.AddAuthorization(options => options.PolicyConfig());
        serviceCollection
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.Config(config));
        serviceCollection.AddSwaggerGen(options => options.Config());
        serviceCollection.AddDataProtection().PersistKeysToDbContext<EFContext>();
        serviceCollection.AddSignalR(
            (options) =>
            {
                options.AddFilter<HubExceptionFilter>();
                options.MaximumReceiveMessageSize = 25000000;
            }
        );
        serviceCollection.AddHttpClient();
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
        });

        return serviceCollection;
    }
}

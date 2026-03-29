using Chat.WebApi.ApiBuilder.ApplicationBuilder;
using Chat.WebApi.ApiBuilder.LoggingBuilder;
using Chat.WebApi.ApiBuilder.ServiceManager;
using Chat.WebApi.Common;

AppEnvironment.LoadEnvironments();


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

builder.Services.AddConfigurationDependencies(config);
builder.Services.AddCommonDependencies(config);
builder.Services.AddTransientDependencies();
builder.Services.AddScopedDependencies();
builder.Services.AddSingletonDependencies(config);
builder.Services.AddHostedDependencies();

builder.Logging.AddCommonConfiguration(config);

WebApplication application = builder.Build();

application.ApplyMigrations();
application.AddEnvironmentConfiguration();
application.CommonConfiguration();
application.HubsConfiguration();
application.SeedsConfiguration();

application.Run();

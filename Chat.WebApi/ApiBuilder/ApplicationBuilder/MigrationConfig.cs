using Chat.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Chat.WebApi.ApiBuilder.ApplicationBuilder;

public static partial class ApplicationBuilderExtension
{
    public static void ApplyMigrations(this WebApplication webApplication)
    {
        using IServiceScope scope = webApplication.Services.CreateScope();
        EFContext db = scope.ServiceProvider.GetRequiredService<EFContext>();
        db.Database.Migrate();
    }
}

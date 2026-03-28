using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chat.Persistence.DBContext;

public class EFContextFactory : IDesignTimeDbContextFactory<EFContext>
{
    public EFContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EFContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=ChatDB;Username=postgres;Password=SecretPassword161");
        return new EFContext(optionsBuilder.Options);
    }
}

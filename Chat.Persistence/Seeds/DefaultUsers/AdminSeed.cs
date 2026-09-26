using Chat.Domain.Entities.Accounts.Admins;
using Chat.Domain.Security;
using Chat.Domain.Shared.Models;
using Chat.Persistence.DBContext;

namespace Chat.Persistence.Seeds.DefaultUsers;

internal static partial class DefaultUsersSeed
{
    public static void CreateAdmins(EFContext eFContext, string adminPassword)
    {
        if (!eFContext.Admins.Any())
        {
            var admins = new[] { new { Email = "admin@admin.com", Login = "Admin" } };

            IEnumerable<Admin> adminList = admins.Select(admin =>
            {
                Password password = PasswordHasher.Create(adminPassword);

                return new Admin(admin.Email, admin.Login, password.Hash, password.Salt);
            });

            eFContext.Admins.AddRange(adminList);
            eFContext.SaveChanges();
        }
    }
}

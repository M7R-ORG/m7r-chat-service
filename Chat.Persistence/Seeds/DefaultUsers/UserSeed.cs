using Chat.Domain.Entities.Accounts.Users;
using Chat.Domain.Security;
using Chat.Domain.Shared.Models;
using Chat.Persistence.DBContext;

namespace Chat.Persistence.Seeds.DefaultUsers;

internal static partial class DefaultUsersSeed
{
    public static void CreateUsers(EFContext eFContext)
    {
        if (!eFContext.Users.Any())
        {
            var users = new[]
            {
                new
                {
                    Email = "user@user.com",
                    Login = "User",
                    Password = "Sosnova61S"
                }
            };

            IEnumerable<User> userList = users.Select(user =>
            {
                Password password = PasswordHasher.Create(user.Password);

                return new User(user.Email, user.Login, password.Hash, password.Salt)
                {
                    Birthday = new DateOnly(2002, 1, 9)
                };
            });

            eFContext.Users.AddRange(userList);
            eFContext.SaveChanges();
        }
    }
}

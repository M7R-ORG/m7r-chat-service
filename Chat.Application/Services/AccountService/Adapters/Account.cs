using Chat.Application.Services.AccountService.Models;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Accounts.Users;

namespace Chat.Application.Services.AccountService.Adapters;

public class AccountServiceAccountAdapter : AccountServiceAccountResponseData
{
    public AccountServiceAccountAdapter(Account account)
    {
        Id = account.Id;
        Login = account.Login;
        Email = account.Email;
        Role = account.Role;
        IsBanned = (account as User)?.IsBanned;
        ActivityStatus = account.ActivityStatus;
        LastOnlineAt = account.LastOnlineAt;
        ImageId = account.Image;
    }
}

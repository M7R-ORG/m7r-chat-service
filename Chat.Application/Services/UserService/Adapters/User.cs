using Chat.Application.Services.UserService.Models;
using Chat.Domain.Entities.Accounts.Users;

namespace Chat.Application.Services.UserService.Adapters;

public class UserServiceUserAdapter : UserServiceUserResponseData
{
    public UserServiceUserAdapter(User user)
    {
        Id = user.Id;
        Login = user.Login;
        Email = user.Email;
        Role = user.Role;
        Birthday = user.Birthday;
        IsBanned = user.IsBanned;
        ActivityStatus = user.ActivityStatus;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
        ImageId = user.Image;
    }
}

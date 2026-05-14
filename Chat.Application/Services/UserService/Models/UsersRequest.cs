using Chat.Domain.Shared.Models;

namespace Chat.Application.Services.UserService.Models;

public class UserServiceUsersRequest
{
    public Pagination? Pagination { get; set; }
}

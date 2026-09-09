using Chat.Domain.Shared.Models;

namespace Chat.WebApi.Controllers.Models.User;

public class UserControllerUsersRequest
{
    public Pagination? Pagination { get; set; }
}

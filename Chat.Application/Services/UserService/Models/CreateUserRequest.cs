namespace Chat.Application.Services.UserService.Models;

public class UserServiceCreateUserRequest
{
    public required string Login { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateOnly? Birthday { get; set; }
}

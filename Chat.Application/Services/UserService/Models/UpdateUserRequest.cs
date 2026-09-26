namespace Chat.Application.Services.UserService.Models;

public class UserServiceUpdateUserRequest
{
    public int UserId { get; set; }
    public required string Login { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; }
    public DateOnly? Birthday { get; set; }
}

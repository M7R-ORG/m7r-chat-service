using Chat.Domain.Shared.Models;

namespace Chat.Application.Services.AccountService.Models;

public class AccountServiceAccountResponseData
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public bool? IsBanned { get; set; }
    public string? ActivityStatus { get; set; }
    public DateTime LastOnlineAt { get; set; }
    public string? ImageId { get; set; }
    public DateOnly? Birthday { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AccountServiceAccountsResponse
{
    public MetaResponse? Meta { get; set; }
    public IEnumerable<AccountServiceAccountResponseData>? Accounts { get; set; }
}

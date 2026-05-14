using Chat.Domain.Shared.Models;

namespace Chat.WebApi.Controllers.Models.Account;

public class AccountControllerAccountsRequest
{
    public Pagination? Pagination { get; set; }
    public string? SearchField { get; set; }
}

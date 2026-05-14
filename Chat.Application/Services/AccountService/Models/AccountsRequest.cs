using Chat.Domain.Shared.Models;

namespace Chat.Application.Services.AccountService.Models;

public class AccountServiceAccountsRequest
{
    public Pagination? Pagination { get; set; }
    public string? SearchField { get; set; }
}

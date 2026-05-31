using Chat.Domain.Shared.Constants.Common;
using Chat.Domain.Specification;

namespace Chat.Domain.Entities.Accounts;

public class AccountByIdSpec : Specification<Account>
{
    public AccountByIdSpec(int id, bool isTracking)
        : base((account) => account.Id == id)
    {
        if (isTracking)
        {
            ApplyTracking();
        }
    }
}

public class AccountsByIdsSpec : Specification<Account>
{
    public AccountsByIdsSpec(IEnumerable<int> ids)
        : base((account) => ids.Contains(account.Id))
    {
        ApplyTracking();
    }
}

public class AccountByEmailSpec : Specification<Account>
{
    public AccountByEmailSpec(string email)
        : base((account) => account.Email == email) { }
}

public class AccountsSpec : Specification<Account>
{
    public AccountsSpec(int accountId, string? searchField)
        : base(
            (account) =>
                account.Id != accountId
                && account.Role != AccountRole.AIBot
                && (searchField == null || account.Login.ToLower().Contains(searchField.ToLower()))
        )
    {
        ApplyOrderBy(account => account.Id);
    }
}

public class FirstAIBotAccountSpec : Specification<Account>
{
    public FirstAIBotAccountSpec()
        : base((account) => account.Role == AccountRole.AIBot)
    {
        ApplyTracking();
    }
}

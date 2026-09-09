using Chat.Domain.Specification;

namespace Chat.Domain.Entities.Accounts.Users;

public class UserByIdSpec : Specification<User>
{
    public UserByIdSpec(int id, bool isTracking)
        : base((user) => user.Id == id)
    {
        if (isTracking)
        {
            ApplyTracking();
        }
    }
}

public class UsersSpec : Specification<User>
{
    public UsersSpec()
    {
        ApplyOrderBy(user => user.Id);
    }
}

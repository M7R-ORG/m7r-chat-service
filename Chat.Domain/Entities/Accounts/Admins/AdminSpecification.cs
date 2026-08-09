using Chat.Domain.Specification;

namespace Chat.Domain.Entities.Accounts.Admins;

public class AdminByIdSpec : Specification<Admin>
{
    public AdminByIdSpec(int id)
        : base((admin) => admin.Id == id) { }
}

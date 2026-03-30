using Chat.Domain.Common;
using Chat.Domain.Entities.Accounts.Users;
using Chat.Domain.Exceptions;
using Chat.Domain.Services.AuthService;
using Chat.Domain.Shared.Models;
using Chat.Domain.Specification;

namespace Chat.Domain.Services.UserService;

public class UserBS : DomainService
{
    public UserBS(IAppSettings appSettings, IUnitOfWork unitOfWork)
        : base(appSettings, unitOfWork) { }

    public async Task<User?> GetUserByIdAsync(int id, bool isTracking = false)
    {
        return await _unitOfWork.User.GetAsync(new UserByIdSpec(id, isTracking));
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _unitOfWork.User.GetAllAsync() ?? throw new NotExistsException("Users");
    }

    public async Task<PaginatorResponse<User>> GetUsersPaginatedAsync(Pagination? pagination)
    {
        return await _unitOfWork.User.GetPaginatedAsync(
            new DefaultSpec<User>(),
            pagination
        );
    }

    public async Task CheckExistenceByEmailAsync(string email)
    {
        if (await _unitOfWork.Account.AnyAsync(account => account.Email == email))
        {
            throw new AlreadyExistsException("Account with this email already exists");
        }
    }

    public async Task BlockUserAsync(User user)
    {
        user.UpdateIsBanned(true);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UnblockUserAsync(User user)
    {
        user.UpdateIsBanned(false);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveUserAsync(User user)
    {
        user.Delete();
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ConfirmRegistrationAsync(Confirmation confirmation)
    {
        if (confirmation.ExpirationDate < DateTime.UtcNow)
            throw new OperationNotAllowedException("Confirmation has expired");

        if (await _unitOfWork.Account.AnyAsync(account => account.Email == confirmation.Email))
            throw new AlreadyExistsException("Account already exists");

        Password password = AuthBS.CreatePasswordHash(confirmation.Password);

        var user = new User(confirmation.Email, confirmation.Login, password.Hash, password.Salt)
        {
            Birthday = confirmation.Birthday,
        };

        await _unitOfWork.User.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user, string login, DateOnly? birthday)
    {
        user.UpdateLogin(login);
        user.UpdateBirthday(birthday);

        _unitOfWork.User.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }
}

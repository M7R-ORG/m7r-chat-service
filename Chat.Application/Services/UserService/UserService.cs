using System.Text.Json;
using Chat.Application.Services.Common;
using Chat.Application.Services.UserService.Adapters;
using Chat.Application.Services.UserService.Models;
using Chat.Domain.Common;
using Chat.Domain.Entities.Accounts.Users;
using Chat.Domain.Exceptions;
using Chat.Domain.Services.UserService;
using Chat.Domain.Shared.Models;
using Chat.Infrastructure.Services.NotificationsService;
using Chat.Infrastructure.Services.NotificationsService.Models;
using Chat.Persistence.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Services.UserService;

public class UserService : BaseService, IUserService
{
    private readonly IDataProtectionProvider _protection;
    private readonly UserBS _userBS;
    private readonly INotificationsIS _notificationsIS;

    public UserService(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor context,
        IAppSettings appSettings,
        IDataProtectionProvider protection,
        UserBS userBS,
        INotificationsIS notificationsIS
    )
        : base(unitOfWork, context, appSettings)
    {
        _protection = protection;
        _userBS = userBS;
        _notificationsIS = notificationsIS;
    }

    public async Task<UserServiceRegistrationResponse> RegistrationAsync(
        UserServiceRegistrationRequest request
    )
    {
        await _userBS.CheckExistenceByEmailAsync(request.Email);

        string baseUrl = _appSettings.Client.BaseUrl;

        string path = _appSettings.RoutePath.ConfirmRegistration;

        string secretKey = _appSettings.Common.SecretKey;

        IDataProtector protector = _protection.CreateProtector(secretKey);

        string confirmationJson = JsonSerializer.Serialize(
            new Confirmation()
            {
                Login = request.Login,
                Email = request.Email,
                Password = request.Password,
                Birthday = request.Birthday,
                ExpirationDate = DateTime.UtcNow.AddHours(1)
            }
        );

        string confirmation = protector.Protect(confirmationJson);

        string confirmationLink = $"{baseUrl}/{path}?code={confirmation}";

        await _notificationsIS.SendEmailAsync(
            new NotificationsIServiceSendEmailRequest()
            {
                Template = EmailTemplate.ConfirmRegistration,
                Recipient = request.Email,
                Data = new { recipientName = request.Login, confirmationLink }
            }
        );

        return new UserServiceRegistrationResponse() { IsSuccess = true };
    }

    public async Task<UserServiceConfirmationResponse> ConfirmationAsync(
        UserServiceConfirmationRequest request
    )
    {
        string secretKey = _appSettings.Common.SecretKey;

        IDataProtector protector = _protection.CreateProtector(secretKey);
        string confirmationJson = protector.Unprotect(request.Confirmation);

        Confirmation confirmation =
            JsonSerializer.Deserialize<Confirmation>(confirmationJson)
            ?? throw new InvalidConfirmationException();

        await _userBS.ConfirmRegistrationAsync(confirmation);

        return new UserServiceConfirmationResponse()
        {
            Email = confirmation.Email,
            Password = confirmation.Password
        };
    }

    public async Task<UserServiceUpdateResponse> UpdateAsync(UserServiceUpdateRequest request)
    {
        User user =
            await _userBS.GetUserByIdAsync(AccountId)
            ?? throw new NotExistsException("User not found");

        await _userBS.UpdateAsync(user, request.Login, request.Birthday);

        return new UserServiceUpdateResponse() { IsSuccess = true };
    }

    public async Task<UserServiceUsersResponse> UsersAsync(UserServiceUsersRequest request)
    {
        PaginatorResponse<User> paginatedData = await _userBS.GetUsersPaginatedAsync(
            request.Pagination
        );

        var adaptedUsers = paginatedData
            .Collection
            .Select(user => new UserServiceUserAdapter(user))
            .ToList();

        if (request.IsLoadImage)
            await Task.WhenAll(adaptedUsers.Select(user => user.LoadImageAsync()));

        return new UserServiceUsersResponse() { Meta = paginatedData.Meta, Users = adaptedUsers };
    }

    public async Task<UserServiceUserResponse> UserAsync(UserServiceUserRequest request)
    {
        User user =
            await _userBS.GetUserByIdAsync(request.Id)
            ?? throw new NotExistsException("User not exists");

        byte[]? image = request.IsLoadImage ? await FileManager.ReadToBytesAsync(user.Image) : null;

        return new UserServiceUserResponse()
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            Role = user.Role,
            Image = image,
            Birthday = user.Birthday,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task<UserServiceBlockUserResponse> BlockUserAsync(
        UserServiceBlockUserRequest request
    )
    {
        User user =
            await _userBS.GetUserByIdAsync(request.UserId, true)
            ?? throw new NotExistsException("User not found");

        await _userBS.BlockUserAsync(user);

        return new UserServiceBlockUserResponse() { IsSuccess = true };
    }

    public async Task<UserServiceUnblockUserResponse> UnblockUserAsync(
        UserServiceUnblockUserRequest request
    )
    {
        User user =
            await _userBS.GetUserByIdAsync(request.UserId, true)
            ?? throw new NotExistsException("User not found");

        await _userBS.UnblockUserAsync(user);

        return new UserServiceUnblockUserResponse() { IsSuccess = true };
    }

    public async Task<UserServiceRemoveUserResponse> RemoveUserAsync(
        UserServiceRemoveUserRequest request
    )
    {
        User user =
            await _userBS.GetUserByIdAsync(request.UserId, true)
            ?? throw new NotExistsException("User not found");

        await _userBS.RemoveUserAsync(user);

        return new UserServiceRemoveUserResponse() { IsSuccess = true };
    }
}

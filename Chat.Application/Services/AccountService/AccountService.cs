using Chat.Application.Services.AccountService.Adapters;
using Chat.Application.Services.AccountService.Extensions;
using Chat.Application.Services.AccountService.Models;
using Chat.Application.Services.Common;
using Chat.Domain.Common;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Accounts.Users;
using Chat.Domain.Exceptions;
using Chat.Domain.Services.AccountService;
using Chat.Domain.Shared.Models;
using Chat.Persistence.Extensions;
using Microsoft.AspNetCore.Http;

namespace Chat.Application.Services.AccountService;

public class AccountService : BaseService, IAccountService
{
    private readonly AccountBS _accountBS;

    public AccountService(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor context,
        IAppSettings appSettings,
        AccountBS accountBS
    )
        : base(unitOfWork, context, appSettings)
    {
        _accountBS = accountBS;
    }

    public async Task<AccountServiceUploadImageResponse> UploadImageAsync(
        AccountServiceUploadImageRequest request
    )
    {
        Account account =
            await _unitOfWork.Account.GetAsync(new AccountByIdSpec(AccountId, true))
            ?? throw new NotExistsException("Account not found");

        string fullPath = await ProcessImage.CreateFileAsync(
            request.Image,
            account.Email,
            _appSettings
        );

        await _accountBS.UpdateImageAsync(account, fullPath);

        return new AccountServiceUploadImageResponse() { Image = request.Image };
    }

    public async Task<AccountServiceImageResponse> GetImageAsync()
    {
        Account account =
            await _accountBS.GetAccountByIdAsync(AccountId)
            ?? throw new NotExistsException("Account not found");

        byte[]? image = await FileManager.ReadToBytesAsync(account.Image);

        return new AccountServiceImageResponse() { Image = image };
    }

    public async Task<AccountServiceUpdateStatusResponse> UpdateStatusAsync(
        AccountServiceUpdateStatusRequest request
    )
    {
        Account account =
            await _accountBS.GetAccountByIdAsync(AccountId)
            ?? throw new NotExistsException("Account not found");

        await _accountBS.UpdateActivityStatusAsync(account, request.ActivityStatus);

        return new AccountServiceUpdateStatusResponse() { IsSuccess = true };
    }

    public async Task<AccountServiceAccountsResponse> AccountsAsync(
        AccountServiceAccountsRequest request
    )
    {
        PaginatorResponse<Account> paginatedData = await _accountBS.GetAccountsPaginatedAsync(
            AccountId,
            request.SearchField,
            request.Pagination
        );

        var adaptedAccounts = paginatedData
            .Collection
            .Select(account => new AccountServiceAccountAdapter(account))
            .ToList();

        if (request.IsLoadImage)
            await Task.WhenAll(adaptedAccounts.Select(account => account.LoadImageAsync()));

        return new AccountServiceAccountsResponse()
        {
            Meta = paginatedData.Meta,
            Accounts = adaptedAccounts
        };
    }

    public async Task<AccountServiceProfileResponse> GetProfileAsync()
    {
        Account account =
            await _accountBS.GetAccountByIdAsync(AccountId)
            ?? throw new NotExistsException("Account not found");

        return new AccountServiceProfileResponse()
        {
            Login = account.Login,
            Email = account.Email,
            Role = account.Role,
            Birthday = (account as User)?.Birthday
        };
    }

    public async Task<AccountServiceAccountImageResponse> GetAccountImageAsync(
        AccountServiceAccountImageRequest request
    )
    {
        Account account =
            await _accountBS.GetAccountByIdAsync(request.AccountId)
            ?? throw new NotExistsException("Account not found");

        byte[]? image = await FileManager.ReadToBytesAsync(account.Image);

        return new AccountServiceAccountImageResponse() { Image = image };
    }

    public async Task<AccountServiceAccountByIdResponse> GetExtendedAccountByIdAsync(
        AccountServiceAccountByIdRequest request
    )
    {
        Account? account =
            await _accountBS.GetAccountByIdAsync(request.AccountId)
            ?? throw new NotExistsException("Account not found");

        return new AccountServiceFullAccountByIdResponse()
        {
            Id = account.Id,
            Login = account.Login,
            Email = account.Email,
            Role = account.Role,
            PasswordHash = account.PasswordHash,
            PasswordSalt = account.PasswordSalt,
            IsBanned = (account as User)?.IsBanned,
        };
    }

    public async Task<AccountServiceAccountByIdResponse> GetAccountByIdAsync(
        AccountServiceAccountByIdRequest request
    )
    {
        Account? account =
            await _accountBS.GetAccountByIdAsync(request.AccountId)
            ?? throw new NotExistsException("Account not found");

        return new AccountServiceExtendedAccountByIdResponse()
        {
            Id = account.Id,
            Login = account.Login,
            Email = account.Email,
            Role = account.Role,
            ActivityStatus = account.ActivityStatus,
            LastOnlineAt = account.LastOnlineAt,
            IsBanned = (account as User)?.IsBanned,
        };
    }

    public async Task<AccountServiceAccountByEmailResponse> GetAccountByEmailAsync(
        AccountServiceAccountByEmailRequest request
    )
    {
        Account? account =
            await _accountBS.GetAccountByEmailAsync(request.Email)
            ?? throw new NotExistsException("Account not found");

        return new AccountServiceAccountByEmailResponse()
        {
            Id = account.Id,
            Login = account.Login,
            Email = account.Email,
            Role = account.Role,
            PasswordHash = account.PasswordHash,
            PasswordSalt = account.PasswordSalt,
            IsBanned = (account as User)?.IsBanned,
        };
    }

    public async Task<AccountServiceUpdatePasswordResponse> UpdatePasswordAsync(
        AccountServiceUpdatePasswordRequest request
    )
    {
        Account account =
            await _accountBS.GetAccountByIdAsync(request.AccountId)
            ?? throw new NotExistsException("Account not found");

        await _accountBS.UpdatePasswordAsync(account, request.Password);

        return new AccountServiceUpdatePasswordResponse() { Password = request.Password };
    }
}

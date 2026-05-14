using Chat.Application.Services.AccountService;
using Chat.Application.Services.AccountService.Models;
using Chat.WebApi.Controllers.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("image"), Authorize]
    public async Task<IActionResult> Image()
    {
        AccountServiceImageResponse response = await _accountService.GetImageAsync();

        return Ok(response);
    }

    [HttpPost("upload-image"), Authorize]
    public async Task<IActionResult> UploadImage(
        [FromBody] AccountControllerUploadImageRequest request
    )
    {
        AccountServiceUploadImageResponse response = await _accountService.UploadImageAsync(
            new AccountServiceUploadImageRequest() { FileId = request.FileId }
        );

        return Ok(response);
    }

    [HttpGet("accounts/{id:int}/image"), Authorize]
    public async Task<IActionResult> GetAccountImage([FromRoute] int id)
    {
        AccountServiceAccountImageResponse response = await _accountService.GetAccountImageAsync(
            new AccountServiceAccountImageRequest() { AccountId = id }
        );

        return Ok(response);
    }

    [HttpGet("accounts/{id:int}"), Authorize]
    public async Task<IActionResult> GetAccount([FromRoute] int id)
    {
        AccountServiceAccountByIdResponse response = await _accountService.GetAccountByIdAsync(
            new AccountServiceAccountByIdRequest() { AccountId = id }
        );

        return Ok(response);
    }

    [HttpGet("accounts"), Authorize]
    public async Task<IActionResult> GetAccounts(
        [FromQuery] AccountControllerAccountsRequest request
    )
    {
        AccountServiceAccountsResponse response = await _accountService.AccountsAsync(
            new AccountServiceAccountsRequest()
            {
                Pagination = request.Pagination,
                SearchField = request.SearchField
            }
        );

        return Ok(response);
    }

    [HttpGet("profile"), Authorize]
    public async Task<IActionResult> Profile()
    {
        AccountServiceProfileResponse response = await _accountService.GetProfileAsync();

        return Ok(response);
    }
}

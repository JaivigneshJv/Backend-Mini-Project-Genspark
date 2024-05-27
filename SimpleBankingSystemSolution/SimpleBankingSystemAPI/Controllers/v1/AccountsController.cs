using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Services;
using System.Security.Claims;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;

    }

    [Authorize]
    [HttpPost("open-account")]
    public async Task<IActionResult> OpenAccount(OpenAccountRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var account = await _accountService.OpenAccountAsync(Guid.Parse(userId)!, request);
        return Ok(account);
    }

    [Authorize]
    [HttpGet("get-account/{accountId}")]
    public async Task<IActionResult> GetAccount(Guid accountId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var account = await _accountService.GetAccountAsync(Guid.Parse(userId)!, accountId);
        return Ok(account);
    }

    [HttpPost("close-request/{accountId}")]
    public async Task<IActionResult> RequestCloseAccount(Guid accountId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _accountService.RequestCloseAccountAsync(Guid.Parse(userId)!, accountId);
        return Ok(new { message = "Close request has been sent" });
    }

    [Authorize]
    [HttpGet("get-all-accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var accounts = await _accountService.GetAccountsAsync(Guid.Parse(userId)!);
        return Ok(accounts);
    }

    [HttpPut("/change-transaction-password{accountId}")]
    public async Task<IActionResult> UpdateAccount(Guid accountId, UpdateAccountRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _accountService.UpdateAccountAsync(Guid.Parse(userId)!, accountId, request);
        return Ok(new
        {
            message = "Password updated successfully"
        });
    }
    [Authorize]
    [HttpPost("request/close-account/{accountId}")]
    public async Task<IActionResult> CloseAccount(Guid accountId, AccountClosingDto request)
    {
        await _accountService.CloseAccountAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, request);
        return Ok(new { message = "Account Close Request Raised!" });
    }
}

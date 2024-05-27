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
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _accountService.OpenAccountAsync(Guid.Parse(userId)!, request);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while opening the account: {ex.Message}");
        }
    }

    [Authorize]
    [HttpGet("get-account/{accountId}")]
    public async Task<IActionResult> GetAccount(Guid accountId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _accountService.GetAccountAsync(Guid.Parse(userId)!, accountId);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving the account: {ex.Message}");
        }
    }

    [HttpPost("close-request/{accountId}")]
    public async Task<IActionResult> RequestCloseAccount(Guid accountId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _accountService.RequestCloseAccountAsync(Guid.Parse(userId)!, accountId);
            return Ok(new { message = "Close request has been sent" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while requesting to close the account: {ex.Message}");
        }
    }

    [Authorize]
    [HttpGet("get-all-accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var accounts = await _accountService.GetAccountsAsync(Guid.Parse(userId)!);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving the accounts: {ex.Message}");
        }
    }

    [HttpPut("/change-transaction-password{accountId}")]
    public async Task<IActionResult> UpdateAccount(Guid accountId, UpdateAccountRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _accountService.UpdateAccountAsync(Guid.Parse(userId)!, accountId, request);
            return Ok(new
            {
                message = "Password updated successfully"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating the account: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPost("request/close-account/{accountId}")]
    public async Task<IActionResult> CloseAccount(Guid accountId, AccountClosingDto request)
    {
        try
        {
            await _accountService.CloseAccountAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, request);
            return Ok(new { message = "Account Close Request Raised!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while closing the account: {ex.Message}");
        }
    }
}

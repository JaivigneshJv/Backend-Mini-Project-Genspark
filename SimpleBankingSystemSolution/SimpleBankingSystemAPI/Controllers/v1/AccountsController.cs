using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Services;
using System.Security.Claims;

/// <summary>
/// API controller for managing accounts.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Opens a new account for the authenticated user.
    /// </summary>
    /// <param name="request">The request data for opening an account.</param>
    /// <returns>The newly opened account.</returns>
    [Authorize]
    [HttpPost("open-account")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            if(ex is UserNotFoundException)
            {
                return NotFound(new { message = ex.Message });
            }
            return StatusCode(500, $"An error occurred while opening the account: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves the account with the specified account ID for the authenticated user.
    /// </summary>
    /// <param name="accountId">The ID of the account to retrieve.</param>
    /// <returns>The retrieved account.</returns>
    [Authorize]
    [HttpGet("get-account/{accountId}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            if(ex is AccountNotFoundException)
            {
                return NotFound(new { message = ex.Message });
            }
            return StatusCode(500, $"An error occurred while retrieving the account: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends a request to close the account with the specified account ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to close.</param>
    /// <returns>A message indicating that the close request has been sent.</returns>
    [HttpPost("close-request/{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            if (ex is AccountNotFoundException)
            {
                return NotFound(new { message = ex.Message });
            }
            return StatusCode(500, $"An error occurred while requesting to close the account: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves all accounts for the authenticated user.
    /// </summary>
    /// <returns>The list of accounts.</returns>
    [Authorize]
    [HttpGet("get-all-accounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Updates the transaction password for the account with the specified account ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to update.</param>
    /// <param name="request">The request data for updating the account.</param>
    /// <returns>A message indicating that the password has been updated successfully.</returns>
    [HttpPut("/change-transaction-password{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            if(ex is AccountNotFoundException)
            {
                return NotFound(new { message = ex.Message });
            }
            return StatusCode(500, $"An error occurred while updating the account: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends a request to close the account with the specified account ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to close.</param>
    /// <param name="request">The request data for closing the account.</param>
    /// <returns>A message indicating that the account close request has been raised.</returns>
    [Authorize]
    [HttpPost("request/close-account/{accountId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CloseAccount(Guid accountId, AccountClosingDto request)
    {
        try
        {
            await _accountService.CloseAccountAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, request);
            return Ok(new { message = "Account Close Request Raised!" });
        }
        catch (Exception ex)
        {
            if(ex is AccountNotFoundException)
            {
                return NotFound(new { message = ex.Message });
            }
            return StatusCode(500, $"An error occurred while closing the account: {ex.Message}");
        }
    }
}

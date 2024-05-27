using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Services;
using System.Security.Claims;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdminController : ControllerBase
    {
        private IAccountService _accountService;
        private IUserService _userService;
        private ITransactionService _transactionService;
        private ILoanServices _loanServices;

        public AdminController(ILoanServices loanServices, ITransactionService transactionService, IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
            _transactionService = transactionService;
            _loanServices = loanServices;

        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-active-user")]
        public async Task<IActionResult> GetAllActiveUsers()
        {
            try
            {
                var users = await _userService.GetAllActiveUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-inactive-user")]
        public async Task<IActionResult> GetAllInActiveUsers()
        {
            try
            {
                var users = await _userService.GetAllInActiveUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("/activate/{userId}")]
        public async Task<IActionResult> ActivateUser(Guid userId)
        {
            try
            {
                await _userService.ActivateUser(userId);
                return Ok(new
                {
                    message = "User has been activated"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-accounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAllAccountsAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-accounts-close-request")]
        public async Task<IActionResult> GetAllCloseRequests()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var requests = await _accountService.GetPendingAccountClosingRequests(Guid.Parse(userId)!);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("request/approve/close-account/{requestId}")]
        public async Task<IActionResult> ApproveCloseRequest(Guid requestId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _accountService.AcceptAccountCloseRequest(Guid.Parse(userId)!, requestId);
                return Ok(new { message = "Close request has been approved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("request/reject/close-account/{requestId}")]
        public async Task<IActionResult> RejectCloseRequest(Guid requestId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _accountService.RejectAccountCloseRequest(Guid.Parse(userId)!, requestId);
                return Ok(new { message = "Close request has been rejected" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/all")]
        public async Task<IActionResult> GetAllTransactionRequests()
        {
            try
            {
                var requests = await _transactionService.GetTransactionRequestAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/pending")]
        public async Task<IActionResult> GetPendingTransactionRequests()
        {
            try
            {
                var requests = await _transactionService.GetPendingTransactionRequestAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/approved")]
        public async Task<IActionResult> GetApprovedTransactionRequests()
        {
            try
            {
                var requests = await _transactionService.GetApprovedTransactionRequestAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/rejected")]
        public async Task<IActionResult> GetRejectedTransactionRequests()
        {
            try
            {
                var requests = await _transactionService.GetRejectedTransactionRequestAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/approve/{requestId}")]
        public async Task<IActionResult> ApproveTransactionRequest(Guid requestId)
        {
            try
            {
                await _transactionService.ApproveTransaction(requestId);
                return Ok(new { message = "Transaction request has been approved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/reject/{requestId}")]
        public async Task<IActionResult> RejectTransactionRequest(Guid requestId)
        {
            try
            {
                await _transactionService.RejectTransaction(requestId);
                return Ok(new { message = "Transaction request has been approved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/pending")]
        public async Task<IActionResult> GetPendingLoanRequests()
        {
            try
            {
                var requests = await _loanServices.GetAllPendingLoansAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("loans/request/approve/{loanId}")]
        public async Task<IActionResult> ApproveLoanRequest(Guid loanId)
        {
            try
            {
                await _loanServices.ApproveLoanAsync(loanId);
                return Ok(new { message = "Loan request has been approved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("loans/request/reject/{loanId}")]
        public async Task<IActionResult> RejectLoanRequest(Guid loanId)
        {
            try
            {
                await _loanServices.RejectLoanAsync(loanId);
                return Ok(new { message = "Loan request has been rejected" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/rejected")]
        public async Task<IActionResult> GetRejectedLoanRequests()
        {
            try
            {
                var requests = await _loanServices.GetAllRejectedLoansAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/opened")]
        public async Task<IActionResult> GetOpenedLoanRequests()
        {
            try
            {
                var requests = await _loanServices.GetAllOpenedLoansAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/closed")]
        public async Task<IActionResult> GetClosedLoanRequests()
        {
            try
            {
                var requests = await _loanServices.GetAllClosedLoansAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

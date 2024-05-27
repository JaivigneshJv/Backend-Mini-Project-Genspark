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

        public AdminController(ILoanServices loanServices,ITransactionService transactionService, IAccountService accountService, IUserService userService)
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
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-active-user")]
        public async Task<IActionResult> GetAllActiveUsers()
        {
            var users = await _userService.GetAllActiveUsersAsync();
            return Ok(users);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-inactive-user")]
        public async Task<IActionResult> GetAllInActiveUsers()
        {
            var users = await _userService.GetAllInActiveUsersAsync();
            return Ok(users);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("/activate/{userId}")]
        public async Task<IActionResult> ActivateUser(Guid userId)
        {
            await _userService.ActivateUser(userId);
            return Ok(new
            {
                message = "User has been activated"
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-accounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-accounts-close-request")]
        public async Task<IActionResult> GetAllCloseRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requests = await _accountService.GetPendingAccountClosingRequests(Guid.Parse(userId)!);
            return Ok(requests);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("request/approve/close-account/{requestId}")]
        public async Task<IActionResult> ApproveCloseRequest(Guid requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _accountService.AcceptAccountCloseRequest(Guid.Parse(userId)!, requestId);
            return Ok(new { message = "Close request has been approved" });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("request/reject/close-account/{requestId}")]
        public async Task<IActionResult> RejectCloseRequest(Guid requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _accountService.RejectAccountCloseRequest(Guid.Parse(userId)!, requestId);
            return Ok(new { message = "Close request has been rejected" });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/all")]
        public async Task<IActionResult> GetAllTransactionRequests()
        {
            var requests = await _transactionService.GetTransactionRequestAsync();
            return Ok(requests);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/pending")]
        public async Task<IActionResult> GetPendingTransactionRequests()
        {
            var requests = await _transactionService.GetPendingTransactionRequestAsync();
            return Ok(requests);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/approved")]
        public async Task<IActionResult> GetApprovedTransactionRequests()
        {
            var requests = await _transactionService.GetApprovedTransactionRequestAsync();
            return Ok(requests);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/rejected")]
        public async Task<IActionResult> GetRejectedTransactionRequests()
        {
            var requests = await _transactionService.GetRejectedTransactionRequestAsync();
            return Ok(requests);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/approve/{requestId}")]
        public async Task<IActionResult> ApproveTransactionRequest(Guid requestId)
        {
            await _transactionService.ApproveTransaction(requestId);
            return Ok(new { message = "Transaction request has been approved" });
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/reject/{requestId}")]
        public async Task<IActionResult> RejectTransactionRequest(Guid requestId)
        {
            await _transactionService.RejectTransaction(requestId);
            return Ok(new { message = "Transaction request has been approved" });
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/pending")]
        public async Task<IActionResult> GetPendingLoanRequests()
        {
            var requests = await _loanServices.GetAllPendingLoansAsync();
            return Ok(requests);
        }
        [Authorize(Policy ="AdminOnly")]
        [HttpPost("loans/request/approve/{loanId}")]
        public async Task<IActionResult> ApproveLoanRequest(Guid loanId)
        {
            await _loanServices.ApproveLoanAsync(loanId);
            return Ok(new { message = "Loan request has been approved" });
        }
        [HttpPost("loans/request/reject/{loanId}")]
        public async Task<IActionResult> RejectLoanRequest(Guid loanId)
        {
            await _loanServices.RejectLoanAsync(loanId);
            return Ok(new { message = "Loan request has been rejected" });
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/rejected")]
        public async Task<IActionResult> GetRejectedLoanRequests()
        {
            var requests = await _loanServices.GetAllRejectedLoansAsync();
            return Ok(requests);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/opened")]
        public async Task<IActionResult> GetOpenedLoanRequests()
        {
            var requests = await _loanServices.GetAllOpenedLoansAsync();
            return Ok(requests);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/closed")]
        public async Task<IActionResult> GetClosedLoanRequests()
        {
            var requests = await _loanServices.GetAllClosedLoansAsync();
            return Ok(requests);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Exceptions;
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
        /// <summary>
        /// Retrieves all users from the user service asynchronously.
        /// </summary>
        /// <returns>The list of all users.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all active users from the user service asynchronously.
        /// </summary>
        /// <returns>The list of all active users.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-active-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all inactive users from the user service asynchronously.
        /// </summary>
        /// <returns>The list of all inactive users.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-inactive-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Activates a user with the specified user ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to activate.</param>
        /// <returns>A message indicating the user has been activated.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("/activate/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                if(ex is UserNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all accounts from the account service asynchronously.
        /// </summary>
        /// <returns>The list of all accounts.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-accounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Retrieves all pending account closing requests for the currently logged-in user asynchronously.
        /// </summary>
        /// <returns>The list of all pending account closing requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-accounts-close-request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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
                if(ex is UserNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                else if (ex is AuthorizationException)
                {
                    return StatusCode(403, new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Approves a pending account closing request with the specified request ID asynchronously.
        /// </summary>
        /// <param name="requestId">The ID of the request to approve.</param>
        /// <returns>A message indicating the close request has been approved.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("request/approve/close-account/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                if(ex is UserNotFoundException || ex is AccountNotFoundException || ex is PendingAccountClosingNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                if(ex is AuthorizationException)
                {
                    return StatusCode(403, new { message = ex.Message });
                }

                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Rejects a pending account closing request with the specified request ID asynchronously.
        /// </summary>
        /// <param name="requestId">The ID of the request to reject.</param>
        /// <returns>A message indicating the close request has been rejected.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("request/reject/close-account/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                if (ex is UserNotFoundException || ex is AccountNotFoundException || ex is PendingAccountClosingNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                if (ex is AuthorizationException)
                {
                    return StatusCode(403, new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all transaction requests from the transaction service asynchronously.
        /// </summary>
        /// <returns>The list of all transaction requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all pending transaction requests from the transaction service asynchronously.
        /// </summary>
        /// <returns>The list of all pending transaction requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all approved transaction requests from the transaction service asynchronously.
        /// </summary>
        /// <returns>The list of all approved transaction requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/approved")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all rejected transaction requests from the transaction service asynchronously.
        /// </summary>
        /// <returns>The list of all rejected transaction requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("transaction/request/rejected")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Approves a pending transaction request with the specified request ID asynchronously.
        /// </summary>
        /// <param name="requestId">The ID of the request to approve.</param>
        /// <returns>A message indicating the transaction request has been approved.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/approve/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ApproveTransactionRequest(Guid requestId)
        {
            try
            {
                await _transactionService.ApproveTransaction(requestId);
                return Ok(new { message = "Transaction request has been approved" });
            }
            catch (Exception ex)
            {
                if(ex is TransactionNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Rejects a pending transaction request with the specified request ID asynchronously.
        /// </summary>
        /// <param name="requestId">The ID of the request to reject.</param>
        /// <returns>A message indicating the transaction request has been rejected.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("transaction/request/reject/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RejectTransactionRequest(Guid requestId)
        {
            try
            {
                await _transactionService.RejectTransaction(requestId);
                return Ok(new { message = "Transaction request has been rejected" });
            }
            catch (Exception ex)
            {
                if (ex is TransactionNotFoundException)
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Retrieves all pending loan requests from the loan service asynchronously.
        /// </summary>
        /// <returns>The list of all pending loan requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/pending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Approves a pending loan request with the specified loan ID asynchronously.
        /// </summary>
        /// <param name="loanId">The ID of the loan request to approve.</param>
        /// <returns>A message indicating the loan request has been approved.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("loans/request/approve/{loanId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Rejects a pending loan request with the specified loan ID asynchronously.
        /// </summary>
        /// <param name="loanId">The ID of the loan request to reject.</param>
        /// <returns>A message indicating the loan request has been rejected.</returns>
        [HttpPost("loans/request/reject/{loanId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all rejected loan requests from the loan service asynchronously.
        /// </summary>
        /// <returns>The list of all rejected loan requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/rejected")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all opened loan requests from the loan service asynchronously.
        /// </summary>
        /// <returns>The list of all opened loan requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/opened")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all closed loan requests from the loan service asynchronously.
        /// </summary>
        /// <returns>The list of all closed loan requests.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/request/closed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Retrieves all loan repayments for the specified account ID asynchronously.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve loan repayments for.</param>
        /// <returns>The list of all loan repayments for the specified account.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("loans/repayments/{loanId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoanRepayments(Guid loanId)
        {
            try
            {
                var requests = await _loanServices.GetLoanRepayments(loanId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

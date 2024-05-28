using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;
using SimpleBankingSystemAPI.Services;
using System;
using System.Security.Claims;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    /// <summary>
    /// Controller for handling transactions.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Deposit funds into an account.
        /// </summary>
        /// <param name="request">The deposit request.</param>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The result of the deposit operation.</returns>
        [Authorize]
        [HttpPost("deposit/{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Depsoit(DepositRequest request, Guid accountId)
        {
            try
            {
                await _transactionService.DepositAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, request);
                return Ok(new { message = "Deposited to account!" });
            }
            catch (Exception ex)
            {
                if(ex is AccountNotFoundException)
                {
                    return NotFound(new { error = ex.Message });
                }
                if(ex is AccountNotActivedException || ex is AccessViolationException)
                {
                    return StatusCode(403,(new { error = ex.Message }));
                }
                if(ex is InvalidTransactionException)
                {
                    return BadRequest(new { error = ex.Message });
                }

                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Withdraw funds from an account.
        /// </summary>
        /// <param name="request">The withdrawal request.</param>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The result of the withdrawal operation.</returns>
        [Authorize]
        [HttpPost("withdraw/{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Withdraw(DepositRequest request, Guid accountId)
        {
            try
            {
                await _transactionService.WithdrawAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, request);
                return Ok(new { message = "Withdrawn from account!" });
            }
            catch (Exception ex)
            {
                if (ex is AccountNotFoundException )
                {
                    return NotFound(new { error = ex.Message });
                }
                if (ex is AccountNotActivedException || ex is AccessViolationException)
                {
                    return StatusCode(403, (new { error = ex.Message }));
                }
                if (ex is InvalidTransactionException || ex is InsufficientFundsException)
                {
                    return BadRequest(new { error = ex.Message });
                }
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Transfer funds between two accounts.
        /// </summary>
        /// <param name="request">The transfer request.</param>
        /// <param name="receiverId">The ID of the receiving account.</param>
        /// <param name="accountId">The ID of the sending account.</param>
        /// <returns>The result of the transfer operation.</returns>
        [Authorize]
        [HttpPost("bank-transfer/{accountId}/{receiverId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BankTransfer(BankTransferRequest request, Guid receiverId, Guid accountId)
        {
            try
            {
                await _transactionService.TransferAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, receiverId, request);
                return Ok(new { message = "Otp sent to Email!" });
            }
            catch (Exception ex)
            {
                if (ex is AccountNotFoundException)
                {
                    return NotFound(new { error = ex.Message });
                }
                if (ex is AccountNotActivedException || ex is AccessViolationException)
                {
                    return StatusCode(403, (new { error = ex.Message }));
                }
                if (ex is InvalidAmountException || ex is InsufficientFundsException)
                {
                    return BadRequest(new { error = ex.Message });
                }
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Verify a transfer transaction using IMPS.
        /// </summary>
        /// <param name="verificationCode">The verification code.</param>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The result of the verification operation.</returns>
        [Authorize]
        [HttpPost("transfer/verify-transaction/{accountId}/{verificationCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyTransferIMPS(string verificationCode, Guid accountId)
        {
            try
            {
                var transactionDetail = await _transactionService.TransferVerificationAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, verificationCode);
                if (transactionDetail.TransactionType == "IMPS")
                {
                    return Ok(new { message = "Transaction Approved!" });
                }
                return Ok(new { message = "Transaction will be Verified and Approved Soon!" });
            }
            catch (Exception ex)
            {
                if (ex is TransactionVerificationNotFoundException || ex is AccountNotFoundException)
                {
                    return NotFound(new { error = ex.Message });
                }
                if (ex is AccountNotActivedException || ex is AccessViolationException)
                {
                    return StatusCode(403, (new { error = ex.Message }));
                }
                if (ex is VerificationCodeExpiredException || ex is InvalidVerificationCodeException)
                {
                    return BadRequest(new { error = ex.Message });
                }
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get all transactions for an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The list of transactions.</returns>
        [Authorize]
        [HttpGet("get-transactions/{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactions(Guid accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                if (ex is TransactionVerificationNotFoundException || ex is AccountNotFoundException)
                {
                    return NotFound(new { error = ex.Message });
                }
                if (ex is AccountNotActivedException || ex is AccessViolationException)
                {
                    return StatusCode(403, (new { error = ex.Message }));
                }
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get transaction requests for an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The list of transaction requests.</returns>
        [Authorize]
        [HttpGet("get-transaction-request/{accountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionRequest(Guid accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionByAccountAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                if (ex is AccountNotFoundException)
                {
                    return NotFound(new { error = ex.Message });
                }
                if (ex is AccountNotActivedException || ex is AccessViolationException)
                {
                    return StatusCode(403, (new { error = ex.Message }));
                }
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}

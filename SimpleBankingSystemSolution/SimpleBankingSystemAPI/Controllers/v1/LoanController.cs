using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.LoanDTOs;
using System.Security.Claims;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    /// <summary>
    /// Controller for managing loans.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanServices _loanServices;

        public LoanController(ILoanServices loanServices)
        {
            _loanServices = loanServices;
        }

        /// <summary>
        /// Get loan details.
        /// </summary>
        /// <param name="request">The interest request.</param>
        /// <returns>The loan details.</returns>
        [HttpPost("get-loan-details")]
        [ProducesResponseType(typeof(LoanDetails),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError)]
        public IActionResult GetLoanDetails(InterestRequest request)
        {
            try
            {
                var loanDetails = _loanServices.GetLoanDetails(request);
                return Ok(loanDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Apply for a loan.
        /// </summary>
        /// <param name="request">The loan request.</param>
        /// <returns>The applied loan.</returns>
        [Authorize]
        [HttpPost("apply-loan")]
        [ProducesResponseType(typeof(LoanRequest),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplyLoan(LoanRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loan = await _loanServices.ApplyLoan(Guid.Parse(userId)!, request);
                return Ok(loan);
            }
            catch (Exception ex)
            {
                if(ex is AccountNotFoundException)
                {
                    return StatusCode(404, ex.Message);
                }
                if(ex is AccessViolationException)
                {
                    return StatusCode(403, ex.Message);
                }
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all loans for an account.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <returns>The list of loans.</returns>
        [Authorize]
        [HttpGet("get-all-account-loans/{accountId}")]
        [ProducesResponseType(typeof(IEnumerable<LoanDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetallAccountLoans(Guid accountId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loans = await _loanServices.GetallAccountLoans(Guid.Parse(userId)!, accountId);
                return Ok(loans);
            }
            catch (Exception ex)
            {
                if (ex is AccountNotFoundException)
                {
                    return StatusCode(404, ex.Message);
                }
                if (ex is AccessViolationException)
                {
                    return StatusCode(403, ex.Message);
                }
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Repay a loan.
        /// </summary>
        /// <param name="loanId">The loan ID.</param>
        /// <param name="request">The loan repayment request.</param>
        /// <returns>The loan repayment details.</returns>
        [Authorize]
        [HttpPost("repay-loan/{loanId}")]
        [ProducesResponseType(typeof(LoanRepaymentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RepayLoan(Guid loanId, LoanRepaymentDto request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loanRepayment = await _loanServices.RepayLoanRequest(Guid.Parse(userId)!, loanId, request);
                return Ok(loanRepayment);
            }
            catch (Exception ex)
            {
                if(ex is LoanNotFoundException || ex is AccountNotFoundException)
                {
                    return StatusCode(404, ex.Message);
                }
                if (ex is AccessViolationException)
                {
                    return StatusCode(403, ex.Message);
                }
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// repayments given to a loan.
        /// </summary>
        /// <param name="loanId">The loan ID.</param>
        /// <returns>The loan repayment details.</returns>
        [Authorize]
        [HttpPost("loan-repayments/{loanId}")]
        [ProducesResponseType(typeof(IEnumerable<LoanRepaymentDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRepaymentsForLoanID(Guid loanId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loanrepayments = await _loanServices.GetAllRepaymentsForLoanID(Guid.Parse(userId)!,loanId);
                return Ok(loanrepayments);
            }catch(Exception ex)
            {
                if (ex is AccessViolationException)
                {
                    return StatusCode(403, ex.Message);
                }
                return StatusCode(500, ex.Message);
            }
        }
    }
}

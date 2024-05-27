using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.LoanDTOs;
using System.Security.Claims;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanServices _loanServices;

        public LoanController(ILoanServices loanServices)
        {
            _loanServices = loanServices;
        }
        [HttpPost("get-loan-details")]
        public IActionResult GetLoanDetails(InterestRequest request)
        {
            var loanDetails = _loanServices.GetLoanDetails(request);
            return Ok(loanDetails);
        }
        [Authorize]
        [HttpPost("apply-loan")]
        public async Task<IActionResult> ApplyLoan(LoanRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loan = await _loanServices.ApplyLoan(Guid.Parse(userId)!, request);
            return Ok(loan);
        }
        [Authorize]
        [HttpGet("get-all-account-loans/{accountId}")]
        public async Task<IActionResult> GetallAccountLoans(Guid accountId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loans = await _loanServices.GetallAccountLoans(Guid.Parse(userId)!, accountId);
            return Ok(loans);
        }
        [Authorize]
        [HttpPost("repay-loan/{loanId}")]
        public async Task<IActionResult> RepayLoan(Guid loanId, LoanRepaymentDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loanRepayment = await _loanServices.RepayLoanRequest(Guid.Parse(userId)!, loanId, request);
            return Ok(loanRepayment);
        }
    }
}

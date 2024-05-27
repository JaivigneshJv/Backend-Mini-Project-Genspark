﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;
using SimpleBankingSystemAPI.Services;
using System.Security.Claims;

namespace SimpleBankingSystemAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpPost("deposit/{accountId}")]
        public async Task<IActionResult> Depsoit(DepositRequest request, Guid accountId)
        {
            try
            {
                await _transactionService.DepositAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, request);
                return Ok(new { message = "Deposited to account!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("withdraw/{accountId}")]
        public async Task<IActionResult> Withdraw(DepositRequest request, Guid accountId)
        {
            try
            {
                await _transactionService.WithdrawAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, request);
                return Ok(new { message = "Withdrawn from account!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("bank-transfer/{accountId}/{receiverId}")]
        public async Task<IActionResult> BankTransfer(BankTransferRequest request, Guid receiverId, Guid accountId)
        {
            try
            {
                await _transactionService.TransferAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId, receiverId, request);
                return Ok(new { message = "Otp sent to Email!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("transfer/verify-transaction/{accountId}/{verificationCode}")]
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
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("get-transactions/{accountId}")]
        public async Task<IActionResult> GetTransactions(Guid accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("get-transaction-request/{accountId}")]
        public async Task<IActionResult> GetTransactionRequest(Guid accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionByAccountAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))!, accountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
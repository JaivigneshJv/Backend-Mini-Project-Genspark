using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;

namespace SimpleBankingSystemAPI.Interfaces.Services
{
    public interface ITransactionService
    {
        Task DepositAsync(Guid userId, Guid accountId, DepositRequest request);
        Task WithdrawAsync(Guid userId, Guid accountId, DepositRequest request);
        Task TransferAsync(Guid userId, Guid accountId, Guid receiverId, BankTransferRequest request);
        Task <TransactionDto>TransferVerificationAsync(Guid userId, Guid accountId,string verificationCode);
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync(Guid userId, Guid accountId);
        Task ApproveTransaction(Guid requestId);
        Task RejectTransaction(Guid requestId);
        Task<IEnumerable<TransactionRequestDto>> GetTransactionRequestAsync();
        Task<IEnumerable<TransactionRequestDto>> GetTransactionByAccountAsync(Guid UserId, Guid accountId);
        Task<IEnumerable<TransactionRequestDto>> GetPendingTransactionRequestAsync();
        Task<IEnumerable<TransactionRequestDto>> GetRejectedTransactionRequestAsync();
        Task<IEnumerable<TransactionRequestDto>> GetApprovedTransactionRequestAsync();
    }
}

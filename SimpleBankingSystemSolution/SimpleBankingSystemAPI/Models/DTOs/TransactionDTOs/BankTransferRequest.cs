namespace SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs
{
    public class BankTransferRequest
    {
        public decimal Amount { get; set; }
        public string? TransactionPassword { get; set; }
        public string? TransactionType { get; set; }

    }
}

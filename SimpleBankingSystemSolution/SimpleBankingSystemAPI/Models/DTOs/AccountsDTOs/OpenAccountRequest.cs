namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class OpenAccountRequest
    {
        public string? AccountType { get; set; }
        public decimal InitialDeposit { get; set; }
        public string? TransactionPassword { get; set; }
    }
}

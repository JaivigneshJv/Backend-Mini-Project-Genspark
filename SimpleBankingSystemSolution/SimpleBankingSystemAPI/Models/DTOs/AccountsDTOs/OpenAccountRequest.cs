using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class OpenAccountRequest
    {
        [Required, MaxLength(20)]
        public string? AccountType { get; set; }
        public decimal InitialDeposit { get; set; }
        public string? TransactionPassword { get; set; }
    }
}

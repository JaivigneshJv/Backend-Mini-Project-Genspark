using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class OpenAccountRequest
    {
        [Required(ErrorMessage = "Account type is required"), MaxLength(20, ErrorMessage = "Account type must not exceed 20 characters")]
        public string? AccountType { get; set; }

        [Required(ErrorMessage = "Initial deposit is required")]
        public decimal InitialDeposit { get; set; }

        [Required(ErrorMessage = "Transaction password is required")]
        public string? TransactionPassword { get; set; }
    }
}

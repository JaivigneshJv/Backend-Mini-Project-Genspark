using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs
{
    public class BankTransferRequest
    {
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [StringLength(50, ErrorMessage = "Transaction password cannot exceed 50 characters")]
        public string? TransactionPassword { get; set; }

        [Required(ErrorMessage = "Transaction type is required")]
        public string? TransactionType { get; set; }
    }
}

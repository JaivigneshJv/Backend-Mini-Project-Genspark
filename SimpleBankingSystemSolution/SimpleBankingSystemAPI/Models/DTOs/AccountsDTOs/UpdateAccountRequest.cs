using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class UpdateAccountRequest
    {
        [Required(ErrorMessage = "Transaction password is required.")]
        public string? TransactionPassword { get; set; }
    }
}

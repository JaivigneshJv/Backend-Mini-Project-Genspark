using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class AccountClosingDto
    {
        [Required(ErrorMessage = "AccountId is required.")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "AccountType is required.")]
        [MaxLength(20, ErrorMessage = "AccountType must not exceed 20 characters.")]
        public string? AccountType { get; set; }

        public string? Description { get; set; }
    }
}
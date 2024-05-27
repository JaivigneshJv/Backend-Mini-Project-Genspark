using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class AccountClosingDto
    {

        [Required]
        public Guid AccountId { get; set; }
        [Required, MaxLength(20)]
        public string? AccountType { get; set; }
        public string? Description { get; set; }

    }
}
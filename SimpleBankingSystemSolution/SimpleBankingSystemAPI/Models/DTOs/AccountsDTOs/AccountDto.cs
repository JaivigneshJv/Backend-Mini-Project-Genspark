using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class AccountDto
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(20)]
        public string AccountType { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}


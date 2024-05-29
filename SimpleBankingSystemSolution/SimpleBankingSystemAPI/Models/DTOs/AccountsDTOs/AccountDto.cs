using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class AccountDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        [MaxLength(20, ErrorMessage = "Account type must not exceed 20 characters")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Balance is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Created date is required")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Updated date is required")]
        public DateTime UpdatedDate { get; set; }

        [Required(ErrorMessage = "Is active status is required")]
        public bool IsActive { get; set; }
    }
}


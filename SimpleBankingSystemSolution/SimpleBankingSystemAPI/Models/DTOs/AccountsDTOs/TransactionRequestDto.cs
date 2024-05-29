using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs
{
    public class TransactionRequestDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "AccountId is required")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "ReceiverId is required")]
        public Guid ReceiverId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "TransactionType is required")]
        public string? TransactionType { get; set; }

        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "IsApproved is required")]
        public bool IsApproved { get; set; }

        [Required(ErrorMessage = "IsRejected is required")]
        public bool IsRejected { get; set; }
    }
}

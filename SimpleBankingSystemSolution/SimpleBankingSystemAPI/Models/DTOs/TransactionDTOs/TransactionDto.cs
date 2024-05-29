using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs
{
    public class TransactionDto
    {
        [Required(ErrorMessage = "AccountId is required")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "ReceiverId is required")]
        public Guid ReceiverId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "TransactionType is required")]
        public string? TransactionType { get; set; }

        [Required(ErrorMessage = "Timestamp is required")]
        public DateTime Timestamp { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "IsRecurring is required")]
        public bool IsRecurring { get; set; }
    }
}

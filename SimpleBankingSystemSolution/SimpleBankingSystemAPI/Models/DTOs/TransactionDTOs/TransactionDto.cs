using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs
{
    public class TransactionDto
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string? TransactionType { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public bool IsRecurring { get; set; }
    }
}

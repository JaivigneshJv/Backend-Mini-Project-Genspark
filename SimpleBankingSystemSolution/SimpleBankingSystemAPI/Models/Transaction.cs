using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystemAPI.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

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

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

        [ForeignKey("ReceiverId")]
        public Account? Receiver { get; set; }

        
    }
}

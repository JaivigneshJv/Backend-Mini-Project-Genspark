using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models
{
    public class TransactionVerification
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
        [Required]
        public string? VerificationCode { get; set; }

        [Required]
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
    }
}

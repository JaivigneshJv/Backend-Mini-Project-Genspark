using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystemAPI.Models
{
    public class Loan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal PendingAmount { get; set; }

        [Required, MaxLength(20)]
        public string? Status { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }

        public DateTime? RepaidDate { get; set; }

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }
        public ICollection<LoanRepayment>? LoanRepayments { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankingSystemAPI.Models
{
    public class LoanRepayment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid LoanId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [ForeignKey("LoanId")]
        public Loan? Loan { get; set; }
    }
}

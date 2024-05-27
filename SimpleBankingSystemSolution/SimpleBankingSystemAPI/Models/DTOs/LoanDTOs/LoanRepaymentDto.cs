using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class LoanRepaymentDto
    {
        [Required]
        public Guid LoanId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

    }
}

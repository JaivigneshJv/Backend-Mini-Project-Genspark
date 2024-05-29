using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class LoanRepaymentDto
    {
        [Required(ErrorMessage = "LoanId is required")]
        public Guid LoanId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "PaymentDate is required")]
        public DateTime PaymentDate { get; set; }

    }
}

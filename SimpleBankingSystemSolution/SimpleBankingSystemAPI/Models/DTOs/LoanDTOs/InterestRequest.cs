using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class InterestRequest
    {
        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Applied date is required")]
        public DateTime AppliedDate { get; set; }

        [Required(ErrorMessage = "Target date is required")]
        public DateTime TargetDate { get; set; }
    }
}

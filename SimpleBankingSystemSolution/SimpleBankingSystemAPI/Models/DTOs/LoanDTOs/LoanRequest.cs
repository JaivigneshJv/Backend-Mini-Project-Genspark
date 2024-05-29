using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class LoanRequest
    {
        [Required(ErrorMessage = "AccountId is required")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "AppliedDate is required")]
        public DateTime AppliedDate { get; set; }

        [Required(ErrorMessage = "TargetDate is required")]
        public DateTime TargetDate { get; set; }
    }
}

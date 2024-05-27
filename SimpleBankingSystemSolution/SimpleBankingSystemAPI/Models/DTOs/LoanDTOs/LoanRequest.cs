using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class LoanRequest
    {

        [Required]
        public Guid AccountId { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }

        [Required]
        public DateTime TargetDate { get; set; }

 

    }
}

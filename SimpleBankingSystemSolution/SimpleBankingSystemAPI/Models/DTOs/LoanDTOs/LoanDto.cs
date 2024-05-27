using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class LoanDto
    {

        [Key]
        public Guid Id { get; set; }

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

    }
}

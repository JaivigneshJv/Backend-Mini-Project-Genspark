using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class LoanDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Pending Amount is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PendingAmount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Applied Date is required")]
        public DateTime AppliedDate { get; set; }

        [Required(ErrorMessage = "Target Date is required")]
        public DateTime TargetDate { get; set; }

        public DateTime? RepaidDate { get; set; }
        public Guid AccountId { get; set; }


    }
}

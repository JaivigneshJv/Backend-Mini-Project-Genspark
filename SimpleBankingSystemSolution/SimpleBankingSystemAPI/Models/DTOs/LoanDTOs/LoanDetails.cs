using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.LoanDTOs
{
    public class LoanDetails
    {
        [Required(ErrorMessage = "Interest rate is required")]
        public decimal interestRate { get; set; }

        [Required(ErrorMessage = "Final amount is required")]
        public decimal finalAmount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs.EmailDTOs
{
    public class VerifyEmailRequest
    {
        [Required]
        public string? VerificationCode { get; set; }
    }
}

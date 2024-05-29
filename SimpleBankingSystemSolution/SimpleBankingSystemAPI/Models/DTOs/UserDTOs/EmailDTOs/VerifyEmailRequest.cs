using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs.EmailDTOs
{
    public class VerifyEmailRequest
    {
        [Required, MinLength(6), MaxLength(6)]
        public string? VerificationCode { get; set; }
    }
}

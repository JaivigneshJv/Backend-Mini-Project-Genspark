using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs
{
    public class UpdateUserEmailRequest
    {
        [Required, EmailAddress, MaxLength(100)]
        public string? NewEmail { get; set; }
    }
}

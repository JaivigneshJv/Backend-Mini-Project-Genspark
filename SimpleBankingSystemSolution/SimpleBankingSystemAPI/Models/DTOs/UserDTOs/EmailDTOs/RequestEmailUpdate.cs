using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs.EmailDTOs
{
    public class RequestEmailUpdate
    {
        [Required, EmailAddress, MaxLength(100)]
        public string? NewEmail { get; set; }
    }
}

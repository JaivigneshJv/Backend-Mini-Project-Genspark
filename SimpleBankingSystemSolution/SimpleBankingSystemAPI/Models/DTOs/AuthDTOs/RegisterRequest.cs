using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AuthDTOs
{
    public class RegisterRequest
    {
        [Required, MaxLength(20)]
        public string? Username { get; set; }
        [Required, PasswordPropertyText, MaxLength(20)]
        public string? Password { get; set; }

        [Required, MaxLength(50)]
        public string? FirstName { get; set; }
        [Required, MaxLength(50)]
        public string? LastName { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string? Email { get; set; }
        [Required, MaxLength(20)]

        public string? Contact { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

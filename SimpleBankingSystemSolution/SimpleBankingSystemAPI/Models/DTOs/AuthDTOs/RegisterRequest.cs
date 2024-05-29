using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.AuthDTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required"), MaxLength(20, ErrorMessage = "Username must be at most 20 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required"), PasswordPropertyText, MaxLength(20, ErrorMessage = "Password must be at most 20 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "First name is required"), MaxLength(50, ErrorMessage = "First name must be at most 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required"), MaxLength(50, ErrorMessage = "Last name must be at most 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email address"), MaxLength(100, ErrorMessage = "Email must be at most 100 characters")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Contact is required"), MaxLength(20, ErrorMessage = "Contact must be at most 20 characters")]
        public string? Contact { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}

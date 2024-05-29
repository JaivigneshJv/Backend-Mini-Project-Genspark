using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(30, ErrorMessage = "Username must be maximum 30 characters.")]
        [MinLength(3, ErrorMessage = "Username must be minimum 3 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(30, ErrorMessage = "First name must be maximum 30 characters.")]
        [MinLength(3, ErrorMessage = "First name must be minimum 3 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(30, ErrorMessage = "Last name must be maximum 30 characters.")]
        [MinLength(3, ErrorMessage = "Last name must be minimum 3 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email must be maximum 100 characters.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Contact is required.")]
        [MaxLength(14, ErrorMessage = "Contact must be maximum 14 characters.")]
        [MinLength(10, ErrorMessage = "Contact must be minimum 10 characters.")]
        public string? Contact { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [MaxLength(20, ErrorMessage = "Role must be maximum 20 characters.")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "IsActive is required.")]
        public bool IsActive { get; set; }
    }
}

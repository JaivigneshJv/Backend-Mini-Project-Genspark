using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs
{
    public class UpdateUserProfileRequest
    {
        [Required(ErrorMessage = "First name is required"), MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required"), MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Contact is required"), MaxLength(20, ErrorMessage = "Contact cannot exceed 20 characters")]
        public string? Contact { get; set; }
    }
}

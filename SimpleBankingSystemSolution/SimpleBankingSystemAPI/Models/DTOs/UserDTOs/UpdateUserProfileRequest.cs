using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs
{
    public class UpdateUserProfileRequest
    {
        [Required, MaxLength(50)]
        public string? FirstName { get; set; }
        [Required, MaxLength(50)]
        public string? LastName { get; set; }
        [Required, MaxLength(20)]
        public string? Contact { get; set; }
    }
}

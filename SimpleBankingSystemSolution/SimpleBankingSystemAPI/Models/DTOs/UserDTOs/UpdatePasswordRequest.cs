using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs;

public class UpdatePasswordRequest
{
    [Required(ErrorMessage = "Old password is required")]
    public string? OldPassword { get; set; }

    [Required(ErrorMessage = "New password is required")]
    public string? NewPassword { get; set; }
}

﻿using System.ComponentModel.DataAnnotations;

namespace SimpleBankingSystemAPI.Models.DTOs.UserDTOs
{
    public class UpdateUserEmailRequest
    {
        [Required(ErrorMessage = "NewEmail is required")]
        [EmailAddress(ErrorMessage = "NewEmail must be a valid email address")]
        [MaxLength(100, ErrorMessage = "NewEmail must not exceed 100 characters")]
        public string? NewEmail { get; set; }
    }
}

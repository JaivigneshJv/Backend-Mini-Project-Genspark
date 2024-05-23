﻿using SimpleBankingSystemAPI.Models.DTOs.AuthDTOs;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
namespace SimpleBankingSystemAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
        Task<UserProfileDto> GetUserAsync(Guid userId);
        Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request);
        Task UpdateUserPasswordAsync(Guid userId, UpdatePasswordRequest request);
    }
}

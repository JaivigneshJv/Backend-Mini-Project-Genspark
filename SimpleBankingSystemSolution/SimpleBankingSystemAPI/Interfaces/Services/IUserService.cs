using SimpleBankingSystemAPI.Models.DTOs.AuthDTOs;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;
namespace SimpleBankingSystemAPI.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserProfileDto> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
        Task<UserProfileDto> GetUserAsync(Guid userId);
        Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request);
        Task UpdateUserPasswordAsync(Guid userId, UpdatePasswordRequest request);
        Task RequestEmailUpdateAsync(Guid userId, string newEmail);
        Task VerifyEmailUpdateAsync(Guid userId, string verificationCode);
        Task <IEnumerable<UserProfileDto>> GetAllAsync();
        Task <IEnumerable <UserProfileDto>> GetAllInActiveUsersAsync();
        Task<IEnumerable<UserProfileDto>> GetAllActiveUsersAsync();
        Task ActivateUser(Guid userId);
    }
}

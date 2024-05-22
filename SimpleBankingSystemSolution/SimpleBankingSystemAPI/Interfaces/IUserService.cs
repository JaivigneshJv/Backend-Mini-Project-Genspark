using SimpleBankingSystemAPI.Models.DTOs;
namespace SimpleBankingSystemAPI.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
    }
}

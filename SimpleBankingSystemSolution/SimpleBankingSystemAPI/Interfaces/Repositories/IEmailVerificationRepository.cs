using SimpleBankingSystemAPI.Models;
using System.Threading.Tasks;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface IEmailVerificationRepository : IRepository<Guid, EmailVerification>
    {
        Task<bool> EmailVerificationExists(Guid userId);
        Task<EmailVerification> GetUserByUserIdAsync(Guid userId);
    }
}
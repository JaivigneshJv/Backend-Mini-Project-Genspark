using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using System;
using System.Threading.Tasks;

namespace SimpleBankingSystemAPI.Repositories
{
    public class EmailVerificationRepository : Repository<Guid, EmailVerification>, IEmailVerificationRepository
    {
        private readonly BankingContext _context;

        public EmailVerificationRepository(BankingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> EmailVerificationExists(Guid userId)
        {
            return await _context.EmailVerifications.AnyAsync(e => e.UserId == userId);
        }

        public async Task<EmailVerification> GetUserByUserIdAsync(Guid userId)
        {
            return await _context.EmailVerifications.SingleOrDefaultAsync(e => e.UserId == userId);
        }
    }
}

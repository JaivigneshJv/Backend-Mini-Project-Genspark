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

        /// <summary>
        /// Checks if an email verification exists for the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if an email verification exists, false otherwise.</returns>
        public async Task<bool> EmailVerificationExists(Guid userId)
        {
            return await _context.EmailVerifications.AnyAsync(e => e.UserId == userId);
        }

        /// <summary>
        /// Retrieves the email verification record for the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The email verification record.</returns>
        public async Task<EmailVerification> GetUserByUserIdAsync(Guid userId)
        {
            return await _context.EmailVerifications.SingleOrDefaultAsync(e => e.UserId == userId);
        }
    }
}

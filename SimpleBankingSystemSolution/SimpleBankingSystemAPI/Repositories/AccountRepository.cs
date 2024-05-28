using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class AccountRepository : Repository<Guid, Account>, IAccountRepository
    {
        private readonly BankingContext _context;

        public AccountRepository(BankingContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of accounts associated with a specific user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of accounts.</returns>
        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(Guid userId)
        {
            return await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
        }
    }
}

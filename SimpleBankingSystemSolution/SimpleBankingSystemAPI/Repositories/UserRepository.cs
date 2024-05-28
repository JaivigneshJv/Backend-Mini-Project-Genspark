using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class UserRepository : Repository<Guid, User>, IUserRepository
    {
        private readonly BankingContext _context;

        public UserRepository(BankingContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The user with the specified username, or null if not found.</returns>
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Checks if a user with the specified username exists.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if a user with the specified username exists, otherwise false.</returns>
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user with the specified email, or null if not found.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves all inactive users.
        /// </summary>
        /// <returns>A collection of all inactive users.</returns>
        public async Task<IEnumerable<User>> GetAllInActiveUsers()
        {
            return await _context.Users.Where(u => u.IsActive == false).ToListAsync();
        }

        /// <summary>
        /// Retrieves all active users.
        /// </summary>
        /// <returns>A collection of all active users.</returns>
        public async Task<IEnumerable<User>> GetAllActiveUsers()
        {
            return await _context.Users.Where(u => u.IsActive == true).ToListAsync();
        }
    }
}

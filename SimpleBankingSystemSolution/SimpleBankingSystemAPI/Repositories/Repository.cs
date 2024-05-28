using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;

namespace SimpleBankingSystemAPI.Repositories
{
    public class Repository<K, T> : IRepository<K, T> where T : class
    {
        private readonly BankingContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(BankingContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Adds a new item to the repository.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>The added item.</returns>
        public async Task<T> Add(T item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Deletes an item from the repository based on the specified key.
        /// </summary>
        /// <param name="key">The key of the item to delete.</param>
        /// <returns>The deleted item, or null if not found.</returns>
        public async Task<T> Delete(K key)
        {
            var entity = await _dbSet.FindAsync(key);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return entity;
        }

        /// <summary>
        /// Updates an existing item in the repository.
        /// </summary>
        /// <param name="item">The item to update.</param>
        /// <returns>The updated item.</returns>
        public async Task<T> Update(T item)
        {
            _dbSet.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        /// <summary>
        /// Retrieves an item from the repository based on the specified key.
        /// </summary>
        /// <param name="key">The key of the item to retrieve.</param>
        /// <returns>The retrieved item, or null if not found.</returns>
        public async Task<T> GetById(K key)
        {
            return await _dbSet.FindAsync(key);
        }

        /// <summary>
        /// Retrieves all items from the repository.
        /// </summary>
        /// <returns>A collection of all items.</returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
    }
}

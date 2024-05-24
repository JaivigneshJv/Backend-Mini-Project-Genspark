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

        public async Task<T> Add(T item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

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

        public async Task<T> Update(T item)
        {
            _dbSet.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> GetById(K key)
        {
            return await _dbSet.FindAsync(key);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly EHealthDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(EHealthDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task AddAsync(T entity) { await _dbSet.AddAsync(entity); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(T entity) { _dbSet.Update(entity); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(T entity) { _dbSet.Remove(entity); await _context.SaveChangesAsync(); }
    }
}
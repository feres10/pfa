using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class OrdonnanceRepository : GenericRepository<Ordonnance>
    {
        public OrdonnanceRepository(EHealthDbContext context) : base(context) { }

        public async Task<Ordonnance?> GetWithLignesAsync(int id)
        {
            return await _dbSet.Include(o => o.Lignes).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Ordonnance>> GetAllWithLignesAsync()
        {
            return await _dbSet.Include(o => o.Lignes).ToListAsync();
        }
    }
}
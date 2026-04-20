using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class StockRepository : GenericRepository<StockMouvement>
    {
        public StockRepository(EHealthDbContext context) : base(context) { }

        public async Task<List<StockMouvement>> GetByMedicamentIdAsync(int medicamentId)
        {
            return await _dbSet.Where(s => s.MedicamentId == medicamentId).ToListAsync();
        }
    }
}
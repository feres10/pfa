using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class MedicamentRepository : GenericRepository<Medicament>
    {
        public MedicamentRepository(EHealthDbContext context) : base(context) { }

        public async Task<List<Medicament>> GetExpiringSoonAsync()
        {
            var now = DateTime.UtcNow;
            return await _dbSet.Where(m => m.DateExpiration <= now.AddMonths(1)).ToListAsync();
        }
    }
}
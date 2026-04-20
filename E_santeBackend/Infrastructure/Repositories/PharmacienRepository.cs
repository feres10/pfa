using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class PharmacienRepository : GenericRepository<Pharmacien>
    {
        public PharmacienRepository(EHealthDbContext context) : base(context) { }

        public async Task<Pharmacien?> GetByCompteIdAsync(int compteId)
        {
            return await _dbSet.Include(p => p.Pharmacie).FirstOrDefaultAsync(p => p.Id == compteId);
        }
    }
}
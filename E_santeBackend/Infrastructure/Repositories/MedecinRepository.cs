using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class MedecinRepository : GenericRepository<Medecin>
    {
        public MedecinRepository(EHealthDbContext context) : base(context) { }

        public override async Task<List<Medecin>> GetAllAsync()
        {
            return await _dbSet.Include(m => m.CompteUtilisateur).ToListAsync();
        }

        public override async Task<Medecin?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(m => m.CompteUtilisateur).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Medecin?> GetByCompteIdAsync(int compteId)
        {
            return await _dbSet.Include(m => m.CompteUtilisateur).FirstOrDefaultAsync(m => m.CompteUtilisateur != null && m.CompteUtilisateur.Id == compteId);
        }
    }
}
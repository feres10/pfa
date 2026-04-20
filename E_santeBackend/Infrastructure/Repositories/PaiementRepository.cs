using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class PaiementRepository : GenericRepository<Paiement>
    {
        public PaiementRepository(EHealthDbContext context) : base(context) { }

        public async Task<List<Paiement>> GetByPatientIdAsync(int patientId)
        {
            return await _dbSet.Where(p => p.PatientId == patientId).ToListAsync();
        }
    }
}
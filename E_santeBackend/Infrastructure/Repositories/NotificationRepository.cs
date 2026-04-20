using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>
    {
        public NotificationRepository(EHealthDbContext context) : base(context) { }

        public async Task<List<Notification>> GetByCompteIdAsync(int compteId)
        {
            return await _dbSet.Where(n => n.CompteUtilisateurId == compteId).ToListAsync();
        }
    }
}